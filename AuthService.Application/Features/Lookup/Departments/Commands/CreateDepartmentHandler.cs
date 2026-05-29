using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Entities;
using MediatR;

namespace AuthService.Application.Features.Lookup.Departments.Commands;

public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public CreateDepartmentHandler(IAuthDbContext context, ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Department
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            TenantId = _tenant.TenantId
        };

        _context.Department.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}