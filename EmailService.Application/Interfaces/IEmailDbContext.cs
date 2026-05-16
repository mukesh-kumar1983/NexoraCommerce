using EmailService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Application.Interfaces;

/// <summary>
/// Abstraction for EmailService database operations.
/// 
/// Purpose:
/// ------------------------------------------------------------
/// Allows Application layer to work without depending on EF Core.
/// Improves testability and clean architecture separation.
/// </summary>
public interface IEmailDbContext
{
    DbSet<FailedEmailMessage> FailedEmailMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}