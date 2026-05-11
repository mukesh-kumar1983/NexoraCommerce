namespace AuthService.API.DTOs.Authentication;

public sealed class RegisterUserRequestDto
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}