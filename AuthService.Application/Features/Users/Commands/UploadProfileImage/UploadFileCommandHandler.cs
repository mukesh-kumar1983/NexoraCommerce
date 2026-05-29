using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.Commands.UploadProfileImage;
using MediatR;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, string>
{
    private readonly IAzureBlobService _blobService;

    public UploadFileCommandHandler(IAzureBlobService blobService)
    {
        _blobService = blobService;
    }

    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        return await _blobService.UploadFileAsync(request.File);
    }
}