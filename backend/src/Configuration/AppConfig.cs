namespace Backend.Configuration;

public class AppConfig
{
    public string ServiceName { get; set; } = "Tesorería Inteligente Grupo Andes";
    public string Version { get; set; } = "1.0.0";
    public int DefaultPageSize { get; set; } = 50;
    public int MaxPageSize { get; set; } = 100;
}
