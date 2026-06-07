using AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Application.Features.Commands;
using NexoraEnterprise.AuthService.Domain;
using NexoraEnterprise.AuthService.Domain.Entities;
using NexoraEnterprise.SharedContracts.Events;

namespace NexoraEnterprise.AuthService.Application.Features.Handlers;

/// <summary>
/// Handles the user registration use case.
/// 
/// This handler is responsible for:
/// ------------------------------------------------------------
/// 1. Validating tenant existence
/// 2. Checking for duplicate users
/// 3. Assigning default role
/// 4. Creating user and related domain entities
/// 5. Persisting data using transactional Unit of Work
/// 6. Publishing a domain event to RabbitMQ after successful registration
/// 
/// Architecture Context:
/// ------------------------------------------------------------
/// This class is part of the Application Layer (CQRS - Command Handler).
/// It orchestrates domain operations but does NOT contain infrastructure logic.
/// 
/// After successful registration, it publishes a UserRegisteredEvent
/// to notify other microservices (e.g., EmailService.Worker).
/// 
/// Event Flow:
/// ------------------------------------------------------------
/// AuthService → Database Transaction → Commit → Publish Event → RabbitMQ → Consumers
/// 
/// Important Design Notes:
/// ------------------------------------------------------------
/// - Business rules are enforced here (duplicate check, tenant validation)
/// - Database operations are wrapped in a transaction (UnitOfWork)
/// - Event publishing happens AFTER successful commit to avoid inconsistent state
/// - This ensures eventual consistency in distributed systems
/// </summary>
public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _publisher;

    public RegisterUserCommandHandler(
        IAuthDbContext context,
        IUnitOfWork unitOfWork,
        IMessagePublisher publisher)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    /// <summary>
    /// Executes the user registration workflow.
    /// </summary>
    /// <param name="request">User registration command containing user details</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>Returns newly created User Id (GUID)</returns>
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // ------------------------------------------------------------
        // 1. Validate Tenant
        // ------------------------------------------------------------
        var tenant = await _context.Tenant
            .FirstOrDefaultAsync(t => t.Name == "Default Tenant" && t.IsActive, cancellationToken)
            ?? throw new Exception("Default Tenant does not exist or is not active");

        // ------------------------------------------------------------
        // 2. Check Duplicate User
        // ------------------------------------------------------------
        var userExists = await _context.Users
            .AnyAsync(x => x.Email == request.Email && x.TenantId == tenant.Id, cancellationToken);

        if (userExists)
            throw new Exception("User already exists with this email.");

        // ------------------------------------------------------------
        // 3. Get Default Role
        // ------------------------------------------------------------
        var role = await _context.Role
            .FirstOrDefaultAsync(r => r.Name == "User" && r.IsActive, cancellationToken)
            ?? throw new Exception("Role 'User' does not exist or is not active");

        // ------------------------------------------------------------
        // 4. Create Domain Entities
        // ------------------------------------------------------------
        var userId = Guid.NewGuid();

        var user = new AppUser
        {
            Id = userId,
            Email = request.Email,
            //FirstName = request.FirstName,
            //LastName = request.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            TenantId = tenant.Id,
            IsActive = true,
            IsLocked = false,
            CreatedBy = "System",
            ModifiedBy = "System"
        };

        var profile = new UserProfile
        {
            Id = userId, // Shared primary key relationship
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = Gender.Male
        };

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            TenantId = tenant.Id
        };

        // ------------------------------------------------------------
        // 5. Transactional Database Operation
        // ------------------------------------------------------------
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.UserProfile.AddAsync(profile, cancellationToken);
            await _context.UserRole.AddAsync(userRole, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            // ------------------------------------------------------------
            // 6. Publish Integration Event (After Commit)
            // ------------------------------------------------------------
            var eventMessage = new UserRegisteredEvent
            {
                UserId = userId.ToString(),
                Email = user.Email,
                FullName = $"{profile.FirstName} {profile.LastName}",
                CreatedAt = DateTime.UtcNow
            };

            _publisher.Publish(eventMessage);

            return user.Id;
        }
        catch
        {
            // Rollback ensures database consistency in case of failure
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}