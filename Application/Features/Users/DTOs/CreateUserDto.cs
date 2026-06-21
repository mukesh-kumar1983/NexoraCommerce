namespace Application.Features.Users.DTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Profile

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }

        public string? Address { get; set; }

        //public string? ProfileImageUrl { get; set; }

        // Employment
        public Guid? DepartmentId { get; set; }
        public Guid? JobTitleId { get; set; }
        public string? EmploymentStatus { get; set; }

        public List<string>? Roles { get; set; }
    }
}
