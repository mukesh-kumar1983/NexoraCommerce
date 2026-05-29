using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Features.Users.Commands.UploadProfileImage;

public class UploadFileCommand : IRequest<string>
{
    public IFormFile File { get; set; } = default!;
}