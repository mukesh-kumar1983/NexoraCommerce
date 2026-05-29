using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task UpdateProfileImageAsync(string userId, string imageUrl);
    }
}
