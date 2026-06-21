using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.JobTitles.Commands.CreateJobTitle;

public class CreateJobTitleCommandHandler
    : IRequestHandler<CreateJobTitleCommand, ApiResponse<Guid>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public CreateJobTitleCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse<Guid>> Handle(
        CreateJobTitleCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new JobTitle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            TenantId = _currentTenant.TenantId
        };

        _context.JobTitles.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(
            entity.Id,
            "Job title created successfully"
        );
    }
}