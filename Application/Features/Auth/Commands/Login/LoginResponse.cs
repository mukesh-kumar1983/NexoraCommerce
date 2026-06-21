namespace Application.Features.Auth.Commands.Login;

public class LoginResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public string? Token { get; set; }

    public Guid? UserId { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }

    public Guid? TenantId { get; set; }
    public string? TenantName { get; set; }
}