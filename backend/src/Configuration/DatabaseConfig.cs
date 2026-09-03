namespace Api.Configuration;

public class DatabaseConfig
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 5432;
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Name};Username={Username};Password={Password}";
}
