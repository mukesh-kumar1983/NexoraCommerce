using Microsoft.AspNetCore.Http;

namespace NexoraEnterprise.AuthService.Application.Common.Interfaces;

public interface IAzureBlobService
{
    Task<string> UploadFileAsync(IFormFile file);
}