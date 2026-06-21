using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.LockUser;

public class LockUserCommandHandler
    : IRequestHandler<LockUserCommand, ApiResponse>
{
    private readonly IAuthDbContext _context;

    public LockUserCommandHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse> Handle(
        LockUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            return ApiResponse.FailureResponse(
                "UserNotFound",
                new List<string> { "User not found" },
                $"User not found with ID: {request.UserId}"
            );
        }

        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(request.LockMinutes);

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse.SuccessResponse("User locked successfully");
    }
}