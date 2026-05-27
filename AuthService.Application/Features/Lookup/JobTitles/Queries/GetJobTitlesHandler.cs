using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Lookup.JobTitles.DTOs;
using AuthService.Application.Features.Lookup.JobTitles.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetJobTitlesHandler : IRequestHandler<GetJobTitlesQuery, List<JobTitleDto>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public GetJobTitlesHandler(IAuthDbContext context, ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<List<JobTitleDto>> Handle(GetJobTitlesQuery request, CancellationToken cancellationToken)
    {
        return await _context.JobTitle
            .Where(x => x.TenantId == _tenant.TenantId)
            .Select(x => new JobTitleDto
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync(cancellationToken);
    }
}