using AuthService.Application.Common.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext2 _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AuthDbContext2 context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public Guid UserId =>
        Guid.TryParse(
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out var id)
            ? id
            : Guid.Empty;

        public async Task UpdateProfileImageAsync(string userId, string imageUrl)
        {

            var user = await _context.UserProfile
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
                throw new Exception("User not found");

            user.ProfileImageUrl = imageUrl;

            await _context.SaveChangesAsync();
        }
    }
}
