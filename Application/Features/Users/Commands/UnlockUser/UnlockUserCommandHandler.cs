using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.UnlockUser;

public class UnlockUserCommandHandler
    : IRequestHandler<UnlockUserCommand, ApiResponse>
{
    private readonly IAuthDbContext _context;

    public UnlockUserCommandHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse> Handle(
        UnlockUserCommand request,
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

        user.LockoutEnd = null;
        user.LockoutEnabled = true; // keep enabled for future lock

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse.SuccessResponse("User unlocked successfully");
    }
}