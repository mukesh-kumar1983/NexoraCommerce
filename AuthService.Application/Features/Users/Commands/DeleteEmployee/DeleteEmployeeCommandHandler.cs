using AuthService.Application.Common.Interfaces;

using MediatR;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IAuthDbContext _context;

    public DeleteEmployeeCommandHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _context.Users.FindAsync(request.Id);

        if (employee == null)
            return false;

        employee.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}