using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Settings;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure.Services;

public class AzureBlobService : IAzureBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureBlobSettings _settings;

    public AzureBlobService(IOptions<AzureBlobSettings> options)
    {
        _settings = options.Value;

        _blobServiceClient = new BlobServiceClient(_settings.ConnectionString);

        if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
        {
            throw new Exception("AzureBlobSettings:ConnectionString is missing");
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {   

        var containerClient = _blobServiceClient.GetBlobContainerClient(_settings.ContainerName);

        await containerClient.CreateIfNotExistsAsync();

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";

        var blobPath = $"users/{fileName}";
        var blobClient = containerClient.GetBlobClient(blobPath);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }
}