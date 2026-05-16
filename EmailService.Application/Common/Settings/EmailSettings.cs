using System.Security.Principal;

namespace EmailService.Application.Common.Settings;

/// <summary>
/// SMTP configuration for sending emails.
/// </summary>
public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;

    public string? DefaultCc { get; set; }

    public string? DefaultBCc { get; set; }

  
}