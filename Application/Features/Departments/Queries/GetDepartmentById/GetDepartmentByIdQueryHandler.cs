using Application.Common.Interfaces;
using Application.Features.Departments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace NexoraEnterprise.AuthService.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, ApiResponse<DepartmentDto?>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public GetDepartmentByIdQueryHandler(IAuthDbContext context, 
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse<DepartmentDto?>> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        //return await _context.Departments
        var result= await _context.Departments
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new DepartmentDto
            {
                Id = x.Id,
                Title = x.Title,
                TenantId = _currentTenant.TenantId
            }).OrderBy(x => x.Title)
            .FirstOrDefaultAsync(cancellationToken);

        if(result == null)
        {
            return ApiResponse<DepartmentDto?>.
                FailureResponse("DEPARTMENT_NOT_FOUND", 
                new List<string> { "Department not found" }, 
                $"Department not found with ID: {request.Id}");
        }

        return ApiResponse<DepartmentDto?>.
            SuccessResponse(result, "Department retrieved successfully");
    }
}