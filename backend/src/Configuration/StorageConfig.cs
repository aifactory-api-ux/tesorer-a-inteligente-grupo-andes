namespace Api.Configuration;

public class StorageConfig
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "bank-statements";
}
