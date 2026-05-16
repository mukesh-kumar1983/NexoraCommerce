using EmailService.Application.Common.Settings;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailService.Worker.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendWelcomeEmail(string toEmail, string fullName)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));

        // 🔥 CC (optional but useful for testing)
        if (!string.IsNullOrWhiteSpace(_settings.DefaultCc))
        {
            email.Cc.Add(MailboxAddress.Parse(_settings.DefaultCc));
            email.Bcc.Add(MailboxAddress.Parse(_settings.DefaultBCc!));
        }

        email.Subject = "Welcome to NexoraCommerce 🎉";

        email.Body = new TextPart("html")
        {
            Text = $@"
                <h2>Welcome {fullName} 🎉</h2>
                <p>Thanks for registering at NexoraCommerce.</p>
                <p>We are happy to have you onboard.</p>
            "
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, false);
        await smtp.AuthenticateAsync(_settings.Username, _settings.Password);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}