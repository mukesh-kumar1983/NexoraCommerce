using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Features.JobTitles.Queries.Models;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace Application.Features.JobTitles.Queries;

public class GetJobTitlesQueryHandler
    : IRequestHandler<GetJobTitlesQuery, ApiResponse<List<JobTitleDto>>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService  _currentTenantService;

    public GetJobTitlesQueryHandler(IAuthDbContext context, 
        ICurrentTenantService currentTenantService)
    {
        _context = context;
        _currentTenantService = currentTenantService;
    }

    public async Task<ApiResponse<List<JobTitleDto>>> Handle(
        GetJobTitlesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _context.JobTitles
            .AsNoTracking()
            .Where(x => x.IsDeleted == false && x.TenantId == _currentTenantService.TenantId)
            .Select(x => new JobTitleDto
            {
                Id = x.Id,
                Title = x.Title,
                TenantId = _currentTenantService.TenantId
            })
            .OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);
            
        if(result == null || !result.Any())
        {
            return ApiResponse<List<JobTitleDto>>.FailureResponse("NoData", new List<string> { "No job titles found." });
        }

        return ApiResponse<List<JobTitleDto>>.SuccessResponse(result, "Job titles retrieved successfully");
    }
}