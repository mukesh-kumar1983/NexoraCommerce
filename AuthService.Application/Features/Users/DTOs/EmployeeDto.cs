namespace AuthService.Application.Features.Users.DTOs
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Gender { get; set; }

        // ✅ UI-friendly (IMPORTANT for Angular)
        public string DepartmentName { get; set; } = default!;
        public string JobTitleName { get; set; } = default!;

        // ✅ Internal reference IDs (for edit/update)
        public Guid? DepartmentId { get; set; }
        public Guid? JobTitleId { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

}

