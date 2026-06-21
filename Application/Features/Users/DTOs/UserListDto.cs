using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.DTOs
{
    internal class UserListDto
    {
        public Guid UserId { get; set; }

        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();

        public string? DepartmentName { get; set; }
        public string? JobTitleName { get; set; }

        public string? EmploymentStatus { get; set; }

        public bool IsLocked { get; set; }
    }
}
