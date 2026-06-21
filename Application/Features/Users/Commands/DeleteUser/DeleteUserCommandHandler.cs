using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler
    : IRequestHandler<DeleteUserCommand, ApiResponse>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public DeleteUserCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId;

        var userProfile = await _context.UserProfiles
            .Include(x => x.User)
            .FirstOrDefaultAsync(x =>
                x.UserId == request.UserId &&
                x.TenantId == tenantId,
                cancellationToken);

        if (userProfile == null)
        {
            return ApiResponse.FailureResponse(
                "UserNotFound",
                new List<string> { "User not found" },
                "User not found");
        }

        // 🔥 SOFT DELETE (your BaseEntity supports this)
        userProfile.IsDeleted = true;
        userProfile.DeletedAt = DateTime.UtcNow;
        userProfile.DeletedBy = tenantId.ToString();

        // optional: also disable login
        userProfile.User.LockoutEnd = DateTimeOffset.MaxValue;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse.SuccessResponse("User deleted successfully");
    }
}