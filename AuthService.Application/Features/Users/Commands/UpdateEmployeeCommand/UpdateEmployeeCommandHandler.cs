using AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Commands.UpdateEmployeeCommand
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IAuthDbContext _context;

        public UpdateEmployeeCommandHandler(IAuthDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // =========================
            // GET USER PROFILE (NOT EMPLOYEE TABLE)
            // =========================
            var profile = await _context.UserProfile
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (profile == null)
                throw new Exception("User profile not found");

            // =========================
            // UPDATE PROFILE DATA
            // =========================
            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;

            profile.PhoneNumber = request.PhoneNumber;
            profile.Address = request.Address;
            profile.City = request.City;
            profile.Country = request.Country;
            profile.Gender = request.Gender;

            profile.DepartmentId = request.DepartmentId;
            profile.JobTitleId = request.JobTitleId;
            profile.ProfileImageUrl = request.ProfileImageUrl;

            // =========================
            // SAVE
            // =========================
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
