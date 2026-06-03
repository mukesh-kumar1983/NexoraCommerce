using MediatR;
using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Features.Users.Commands.CreateEmployeeCommand;

public class UpsertEmployeeCommandHandler
    : IRequestHandler<UpsertEmployeeCommand, Guid>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public UpsertEmployeeCommandHandler(IAuthDbContext context, ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<Guid> Handle(
        UpsertEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            // UPDATE
            var employee = await _context.UserProfile
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id.Value,
                    cancellationToken);

            if (employee == null)
                throw new Exception("Employee not found");

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            //employee.Email = request.Email;
            employee.PhoneNumber = request.PhoneNumber;
            employee.Address = request.Address;
            employee.City = request.City;
            employee.Country = request.Country;
            employee.Gender = request.Gender;
            employee.DepartmentId = request.DepartmentId;
            employee.JobTitleId = request.JobTitleId;
            employee.TenantId = _tenant.TenantId; // Keep the existing TenantId
            //employee.ProfileImageUrl = request.ProfileImageUrl;

            await _context.SaveChangesAsync(cancellationToken);

            return employee.Id;
        }

        // CREATE

        var emp = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
            TenantId = _tenant.TenantId

        };

        var newEmployee = new UserProfile
        {
            Id = emp.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            //Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
            Gender = request.Gender,
            DepartmentId = request.DepartmentId,
            JobTitleId = request.JobTitleId,
            ProfileImageUrl = request.ProfileImageUrl,
            TenantId = emp.TenantId
        };

        _context.Users.Add(emp);
        _context.UserProfile.Add(newEmployee);

        await _context.SaveChangesAsync(cancellationToken);

        return newEmployee.Id;
    }
}