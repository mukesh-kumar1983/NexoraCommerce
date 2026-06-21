using Application.Common.Interfaces;
using Application.Features.Departments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Departments.Queries;

public class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, ApiResponse<List<DepartmentDto>>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public GetDepartmentsQueryHandler(IAuthDbContext context, ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse<List<DepartmentDto>>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    { 
        var departments = await _context.Departments
            .AsNoTracking()
            .Where(x => x.IsDeleted == false)
             // Soft delete filter
            .Select(x => new DepartmentDto
            {
                Id = x.Id,
                Title = x.Title,
                TenantId = _currentTenant.TenantId
            }).OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);

        return ApiResponse<List<DepartmentDto>>.
            SuccessResponse(departments, 
            "Departments retrieved successfully");
    }
}
    
