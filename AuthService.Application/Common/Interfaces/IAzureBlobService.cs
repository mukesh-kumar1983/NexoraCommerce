using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Common.Interfaces;

public interface IAzureBlobService
{
    Task<string> UploadFileAsync(IFormFile file);
}