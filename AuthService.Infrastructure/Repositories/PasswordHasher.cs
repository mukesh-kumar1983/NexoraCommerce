using AuthService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastruc.Features.Repositories
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            // HashPassword generates a random salt and applies it automatically
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            // Verify checks the password against the stored hash (which includes the salt)
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
