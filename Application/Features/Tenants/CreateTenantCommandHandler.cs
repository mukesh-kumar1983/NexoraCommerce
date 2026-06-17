using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tenants;

public class CreateTenantCommandHandler
    : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTenantCommandHandler(
        ITenantRepository tenantRepository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateTenantCommand request,
        CancellationToken cancellationToken)
    {
        // ----------------------------------------------------
        // 1. Validate uniqueness (business rule)
        // ----------------------------------------------------
        var exists = await _tenantRepository
            .ExistsBySubdomainAsync(request.Subdomain);

        if (exists)
            throw new Exception("Subdomain already exists");

        // ----------------------------------------------------
        // 2. Create Tenant
        // ----------------------------------------------------
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Subdomain = request.Subdomain,
            IsActive = true
        };

        await _tenantRepository.AddAsync(tenant);

        // ----------------------------------------------------
        // 3. Create Admin User in Identity
        // ----------------------------------------------------
        var userId = await _identityService.CreateUserAsync(
            request.AdminEmail,
            request.AdminPassword,
            tenant.Id);

        await _identityService.AddToRoleAsync(
            userId,
            "TenantAdmin");

        // ----------------------------------------------------
        // 4. Commit Transaction
        // ----------------------------------------------------
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tenant.Id;
    }
}