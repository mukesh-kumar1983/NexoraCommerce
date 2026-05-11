using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Commands;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IAuthDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Tenant
        var tenant = await _context.Tenant
            .FirstOrDefaultAsync(t => t.Name == "Default Tenant" && t.IsActive, cancellationToken)
            ?? throw new Exception("Default Tenant does not exist or is not active");

        // 2. Check duplicate user
        var userExists = await _context.Users
            .AnyAsync(x => x.Email == request.Email && x.TenantId == tenant.Id, cancellationToken);

        if (userExists)
            throw new Exception("User already exists with this email.");

        // 3. Get Role
        var role = await _context.Role
            .FirstOrDefaultAsync(r => r.Name == "User" && r.IsActive, cancellationToken)
            ?? throw new Exception("Role 'User' does not exist or is not active");

        // 4. Create User
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            TenantId = tenant.Id,
            IsActive = true,
            IsLocked = false,
            CreatedBy = "System",
            ModifiedBy = "System"
        };

        // 5. UserRole mapping
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            TenantId = tenant.Id
        };

        // 6. Transaction (via UnitOfWork)
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.UserRole.AddAsync(userRole, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return user.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}