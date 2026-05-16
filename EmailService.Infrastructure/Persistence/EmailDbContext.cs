using EmailService.Application.Interfaces;
using EmailService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EmailService.Infrastructure.Persistence;

/// <summary>
/// Database context for EmailService.
/// 
/// Purpose:
/// ------------------------------------------------------------
/// This context is responsible for persisting email-related data:
/// 
/// - Failed email messages (DLQ storage)
/// - Future: email logs, templates, audit history
/// 
/// Architecture Role:
/// ------------------------------------------------------------
/// Infrastructure layer implementation of persistence.
/// Exposed to Application layer via abstraction (if needed later).
/// </summary>
public class EmailDbContext : DbContext, IEmailDbContext
{
    public EmailDbContext(DbContextOptions<EmailDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Stores failed email messages for DLQ and replay operations.
    /// </summary>
    public DbSet<FailedEmailMessage> FailedEmailMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FailedEmailMessage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(x => x.Payload)
                .IsRequired();

            entity.Property(x => x.ErrorMessage)
                .HasMaxLength(2000);

            entity.Property(x => x.FailedAt)
                .IsRequired();

            entity.Property(x => x.IsReprocessed)
                .HasDefaultValue(false);
        });
    }
}