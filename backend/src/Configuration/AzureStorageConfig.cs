namespace Backend.Configuration;

public class AzureStorageConfig
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountKey { get; set; } = string.Empty;
}
