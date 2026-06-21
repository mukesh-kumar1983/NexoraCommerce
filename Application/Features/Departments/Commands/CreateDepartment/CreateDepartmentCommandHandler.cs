using MediatR;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Features.Departments.Commands;

public class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, ApiResponse<Guid>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenantService;

    public CreateDepartmentCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService currentTenantService)
    {
        _context = context;
        _currentTenantService = currentTenantService;
    }

    public async Task<ApiResponse<Guid>> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new Department
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            TenantId = _currentTenantService.TenantId
        };

        _context.Departments.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(
            entity.Id,
            "Department created successfully"
        );
    }
}