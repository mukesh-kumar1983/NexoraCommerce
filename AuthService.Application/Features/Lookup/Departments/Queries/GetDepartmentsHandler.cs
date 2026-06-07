using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Application.Features.Lookup.Departments.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace NexoraEnterprise.AuthService.Application.Features.Lookup.Departments.Queries;

public class GetDepartmentsHandler : IRequestHandler<GetDepartmentsQuery, List<DepartmentDto>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public GetDepartmentsHandler(IAuthDbContext context, ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<List<DepartmentDto>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenant.TenantId;

        return await _context.Department
            .Where(x => x.TenantId == tenantId)
            .Select(x => new DepartmentDto
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync(cancellationToken);
    }
}