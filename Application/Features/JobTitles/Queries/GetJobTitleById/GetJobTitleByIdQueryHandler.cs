using Application.Common.Interfaces;
using Application.Features.JobTitles.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.JobTitles.Queries;

public class GetJobTitleByIdQueryHandler
    : IRequestHandler<GetJobTitleByIdQuery, ApiResponse<JobTitleDto?>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenantService;

    public GetJobTitleByIdQueryHandler(IAuthDbContext context,
            ICurrentTenantService currentTenantService)
    {
        _context = context;
        _currentTenantService = currentTenantService;
    }

    public async Task<ApiResponse<JobTitleDto?>> Handle(
        GetJobTitleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _context.JobTitles
            .AsNoTracking()
            .Where(
            x => x.Id == request.Id 
            && x.TenantId == _currentTenantService.TenantId 
            && x.IsDeleted == false)
            .Select(x => new JobTitleDto
            {
                Id = x.Id,
                TenantId = _currentTenantService.TenantId,
                Title = x.Title,
            })
            .OrderBy(x => x.Title)  
            .FirstOrDefaultAsync(cancellationToken);

        if(result == null)
        {
            return ApiResponse<JobTitleDto?>.
                FailureResponse("JOB_TITLE_NOT_FOUND", 
                new List<string> { "Job title not found" }, 
                $"Job title not found with ID: {request.Id}");
        }

        return ApiResponse<JobTitleDto?>.
            SuccessResponse(result, "Job title retrieved successfully");
    }
}