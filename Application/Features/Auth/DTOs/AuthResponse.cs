namespace Application.Features.Auth.DTOs;

public class AuthResponse
{
    public string AccessToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }

    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }

    public List<string> Roles { get; set; } = new();
}