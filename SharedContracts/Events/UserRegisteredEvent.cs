using System;

namespace SharedContracts.Events;

/// <summary>
/// Event published when a new user successfully registers in the system.
/// 
/// This is a cross-service contract used in a distributed architecture:
/// 
/// AuthService (Producer)
///     → Publishes this event to RabbitMQ after successful user registration
/// 
/// EmailService.Worker (Consumer)
///     → Consumes this event to perform actions such as sending welcome emails
/// 
/// Why this exists:
/// ------------------------------------------------------------
/// In microservice architecture, services should NOT communicate directly.
/// Instead, they exchange events through a message broker (RabbitMQ).
/// 
/// This enables:
/// - Loose coupling between services
/// - Asynchronous processing (faster API response)
/// - Scalability (multiple consumers can react independently)
/// - Reliability through message queuing
/// 
/// Design Rule:
/// ------------------------------------------------------------
/// This class is a DATA CONTRACT ONLY.
/// It should NOT contain business logic.
/// It must remain stable and backward-compatible.
/// </summary>
public class UserRegisteredEvent
{
    /// <summary>
    /// Unique identifier of the registered user.
    /// Stored as string to ensure cross-service compatibility.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Email address of the user.
    /// Used by downstream services (e.g., EmailService).
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Full name of the user at the time of registration.
    /// Used for personalization in notifications and emails.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// UTC timestamp when the user was registered.
    /// Ensures consistency across distributed services.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}