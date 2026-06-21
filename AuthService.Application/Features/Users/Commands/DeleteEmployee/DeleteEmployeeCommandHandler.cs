using AuthService.Application.Common.Interfaces;

using MediatR;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IAuthDbContext _context;
    private ICurrentUserService _currentUserService;

    public DeleteEmployeeCommandHandler(IAuthDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _context.Users.FindAsync(request.Id);

        if (employee == null)
            return false;

        employee.UserProfile.IsDeleted = true;
        employee.UserProfile.DeletedAt= DateTime.UtcNow;
        employee.UserProfile.DeletedBy = _currentUserService.Email; 

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}