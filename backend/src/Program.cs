using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Backend.Data;
using Backend.Services;
using Backend.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAnalista", policy => policy.RequireRole("Analista"));
    options.AddPolicy("RequireGerente", policy => policy.RequireRole("Gerente"));
    options.AddPolicy("RequireCfo", policy => policy.RequireRole("Cfo"));
    options.AddPolicy("RequireAuditor", policy => policy.RequireRole("Auditor"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireGerenteOrCfo", policy => policy.RequireRole("Gerente", "Cfo"));
    options.AddPolicy("RequireAdminOrCfo", policy => policy.RequireRole("Admin", "Cfo"));
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubsidiaryService, SubsidiaryService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IBankStatementService, BankStatementService>();
builder.Services.AddScoped<IExpectedCollectionService, ExpectedCollectionService>();
builder.Services.AddScoped<IPaymentRequestService, PaymentRequestService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ICashFlowService, CashFlowService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Tesorería Inteligente API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue<string>("AllowedFrontendOrigin") ?? "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

var serviceName = builder.Configuration.GetValue<string>("App:ServiceName") ?? "Tesorería Inteligente Grupo Andes";
var serviceVersion = builder.Configuration.GetValue<string>("App:Version") ?? "1.0.0";

app.MapGet("/health", () => new { status = "healthy", service = serviceName, version = serviceVersion });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRoleAuthorization();
app.MapControllers();

app.Run();
