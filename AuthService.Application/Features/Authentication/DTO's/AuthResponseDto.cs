using System.Runtime.CompilerServices;

namespace AuthService.Application.Features.Authentication.DTOs
{
    public sealed class AuthResponseDto
    {
        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        //public UserProfile UserProfile { get; set; }

        //public string FullName => $"{FirstName} {LastName}";

        public List<string> Roles { get; set; } = new();

    }
}
