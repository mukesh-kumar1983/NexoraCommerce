using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.DTOs
{
    public class EmployeeReportDto
    {
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
    }
}
