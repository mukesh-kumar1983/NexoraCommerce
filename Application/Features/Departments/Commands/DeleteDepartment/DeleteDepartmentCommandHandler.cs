using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler
    : IRequestHandler<DeleteDepartmentCommand, ApiResponse>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public DeleteDepartmentCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse> Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId;

        var entity = await _context.Departments
            .FirstOrDefaultAsync(x =>
                x.Id == request.Id &&
                x.TenantId == tenantId,
                cancellationToken);

        if (entity == null)
        {
            return ApiResponse.FailureResponse(
                "DepartmentNotFound",
                new List<string> { "Department not found" },
                $"Department not found with ID: {request.Id}"
            );
        }

        _context.Departments.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse.SuccessResponse("Department deleted successfully");
    }
}