namespace EmailService.Domain.Entities;

/// <summary>
/// Represents a permanently failed email processing attempt.
///
/// Purpose:
/// ------------------------------------------------------------
/// Stores failed email events after retry exhaustion.
/// These records support:
///
/// - Dead Letter Queue (DLQ) monitoring
/// - Replay/reprocessing
/// - Failure auditing
/// - Operational troubleshooting
///
/// Architecture Role:
/// ------------------------------------------------------------
/// This entity belongs to the Domain layer because it represents
/// persisted business/application state.
/// </summary>
public class FailedEmailMessage
{
    /// <summary>
    /// Unique identifier for failed message record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Recipient email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Original RabbitMQ message payload.
    /// Stored for replay/debugging.
    /// </summary>
    public string Payload { get; set; }

    /// <summary>
    /// Error generated during processing.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// UTC timestamp when failure occurred.
    /// </summary>
    public DateTime FailedAt { get; set; }

    /// <summary>
    /// Indicates whether the failed message
    /// has already been replayed/reprocessed.
    /// </summary>
    public bool IsReprocessed { get; set; }

    public int RetryCount { get; set; } = 0;
}