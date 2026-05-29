namespace AuthService.Application.Common.Settings;

public class AzureBlobSettings
{
    public string ConnectionString { get; set; } = default!;
    public string ContainerName { get; set; } = default!;
}