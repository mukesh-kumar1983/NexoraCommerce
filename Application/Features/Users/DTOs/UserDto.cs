namespace Application.Features.Users.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        // Identity
        public string Email { get; set; } = string.Empty;

        // Profile
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImageUrl { get; set; }

        // Employment
        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public Guid? JobTitleId { get; set; }
        public string? JobTitleName { get; set; }

        public string? EmploymentStatus { get; set; }

        // Security
        public bool IsLocked { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}
