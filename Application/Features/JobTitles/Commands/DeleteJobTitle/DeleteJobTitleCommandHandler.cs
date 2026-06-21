using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.JobTitles.Commands.DeleteJobTitle;

public class DeleteJobTitleCommandHandler
    : IRequestHandler<DeleteJobTitleCommand, ApiResponse>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public DeleteJobTitleCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse> Handle(
        DeleteJobTitleCommand request,
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

        _context.JobTitles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse.SuccessResponse("Job title deleted successfully");
    }
}