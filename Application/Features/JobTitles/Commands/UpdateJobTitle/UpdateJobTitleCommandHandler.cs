using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.JobTitles.Commands.UpdateJobTitle;

public class UpdateJobTitleCommandHandler
    : IRequestHandler<UpdateJobTitleCommand, ApiResponse>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public UpdateJobTitleCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse> Handle(
        UpdateJobTitleCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.JobTitles
            .FirstOrDefaultAsync(x =>
                x.Id == request.Id &&
                x.TenantId == _currentTenant.TenantId,
                cancellationToken);

        if (entity == null)
        {
            return ApiResponse.FailureResponse(
                "JobTitleNotFound",
                new List<string> { "Job title not found" },
                $"Job title not found with ID: {request.Id}"
            );
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse.SuccessResponse("Job title updated successfully");
    }
}