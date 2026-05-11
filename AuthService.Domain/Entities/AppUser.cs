using SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class AppUser : BaseEntity
    {
       
        public string Email { get; set; } = default!;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string PasswordHash { get; set; } = default!;

        
        public bool IsLocked { get; set; } = false;

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        

        public Guid TenantId { get; set; }

        

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
