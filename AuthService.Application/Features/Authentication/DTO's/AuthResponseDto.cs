namespace AuthService.Application.Features.Authentication.DTOs
{
    public sealed class AuthResponseDto
    {
        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
