namespace EmailService.Worker.Services;

public interface IEmailService
{
    Task SendWelcomeEmail(string toEmail, string fullName);
}