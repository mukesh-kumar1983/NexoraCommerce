namespace AuthService.API.DTOs.Authentication;

public sealed class AuthResponseDto
{
    public string Token { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}