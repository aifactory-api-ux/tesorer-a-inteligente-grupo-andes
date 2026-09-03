using System.Security.Claims;
using Backend.Models.Enums;

namespace Backend.Middleware;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RoleAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var objectIdentifier = context.User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier")
                                   ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(objectIdentifier))
            {
                context.Items["AzureAdObjectId"] = objectIdentifier;
            }
        }

        await _next(context);
    }
}

public static class RoleAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseRoleAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RoleAuthorizationMiddleware>();
    }
}
