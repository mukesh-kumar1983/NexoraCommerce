using EmailService.Application.Interfaces;
using EmailService.Worker.Services;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.SharedContracts.Events;
using System.Text.Json;

namespace EmailService.Worker.Processors;

/// <summary>
/// Background retry processor for failed emails stored in SQL.
/// 
/// FLOW:
/// Failed Email (DB) → Retry → SMTP Send → Mark Success / Update Error
/// </summary>
public class EmailRetryProcessor
{
    private readonly IEmailDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailRetryProcessor> _logger;

    // =========================
    // RETRY POLICY CONFIG
    // =========================
    private readonly int _maxRetryCount = 5;

    public EmailRetryProcessor(
        IEmailDbContext dbContext,
        IEmailService emailService,
        ILogger<EmailRetryProcessor> logger)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Processes failed emails from SQL and retries sending them.
    /// </summary>
    public async Task ProcessAsync(CancellationToken token)
    {
        // =========================
        // FETCH FAILED EMAILS
        // =========================
        var failedEmails = await _dbContext.FailedEmailMessages
            .Where(x => !x.IsReprocessed && x.RetryCount < _maxRetryCount)
            .OrderBy(x => x.FailedAt)
            .Take(20)
            .ToListAsync(token);

        if (failedEmails.Count == 0)
        {
            _logger.LogInformation("No failed emails found for retry.");
            return;
        }

        foreach (var email in failedEmails)
        {
            try
            {
                _logger.LogInformation("Retrying email: {Email}, Attempt: {RetryCount}",
                    email.Email,
                    email.RetryCount + 1);

                // =========================
                // IMPORTANT: Deserialize payload
                // =========================
                var userEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(email.Payload);

                if (userEvent == null)
                {
                    _logger.LogWarning("Invalid payload for email: {Email}", email.Email);

                    email.RetryCount++;
                    email.ErrorMessage = "Invalid payload (deserialization failed)";
                    continue;
                }

                // =========================
                // RESEND EMAIL
                // =========================
                await _emailService.SendWelcomeEmail(
                    userEvent.Email,
                    userEvent.FullName);

                // =========================
                // SUCCESS STATE
                // =========================
                email.IsReprocessed = true;
                email.ErrorMessage = null;

                _logger.LogInformation("SUCCESS retry for {Email}", email.Email);
            }
            catch (Exception ex)
            {
                // =========================
                // FAILURE STATE
                // =========================
                email.RetryCount++;
                email.ErrorMessage = ex.Message;

                _logger.LogError(ex,
                    "FAILED retry for {Email}, attempt {RetryCount}",
                    email.Email,
                    email.RetryCount);
            }
        }

        // =========================
        // SAVE CHANGES ONCE (IMPORTANT OPTIMIZATION)
        // =========================
        await _dbContext.SaveChangesAsync(token);
    }
}