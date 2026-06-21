using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;

namespace Application.Features.Departments.Commands;

public class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, ApiResponse>
{
    private readonly IAuthDbContext _context;

    public UpdateDepartmentCommandHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse> Handle(UpdateDepartmentCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = await _context.Departments
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == request.TenantId,
                cancellationToken);

        if (entity == null)
        {
            return ApiResponse.FailureResponse(
                "DepartmentNotFound",
                new List<string> { "Department not found" },
                $"Department not found with ID: {request.Id}"
            );
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse.SuccessResponse("Department updated successfully");
    }
}