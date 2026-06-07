using NexoraEnterprise.AuthService.Domain.Entities;

namespace AuthService.API.DTO_s.Authentication
{
    public class UserProfileDto
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public string? Gender { get; set; }

        // Relations
        public Guid? JobTitleId { get; set; }
        public JobTitle? JobTitle { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Media
        public string? ProfileImageUrl { get; set; }
    }
}
