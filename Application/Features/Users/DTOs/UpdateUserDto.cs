namespace Application.Features.Users.DTOs
{
    internal class UpdateUserDto
    {
        public Guid UserId { get; set; }

        // Profile
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? ProfileImageUrl { get; set; }

        // Employment
        public Guid? DepartmentId { get; set; }
        public Guid? JobTitleId { get; set; }
        public string? EmploymentStatus { get; set; }
    }
}
