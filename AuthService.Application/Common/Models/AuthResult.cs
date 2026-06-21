namespace Application.Common.Models;

public class AuthResult
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? AccessToken { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public Guid? UserId { get; set; }

    public Guid? TenantId { get; set; }

    public List<string> Roles { get; set; } = new();
}