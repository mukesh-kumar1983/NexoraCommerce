using MediatR;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Domain;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Commands;

public class UpsertEmployeeCommandHandler
    : IRequestHandler<UpsertEmployeeCommand, ApiResponse<Guid>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public UpsertEmployeeCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<ApiResponse<Guid>> Handle(
        UpsertEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var employee = await _context.UserProfile
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id.Value,
                    cancellationToken);

            if (employee == null)
                throw new Exception("Employee not found");

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.PhoneNumber = request.PhoneNumber;
            employee.Address = request.Address;
            employee.City = request.City;
            employee.Country = request.Country;
            employee.Gender = request.Gender;
            employee.DepartmentId = request.DepartmentId;
            employee.JobTitleId = request.JobTitleId;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<Guid>.SuccessResponse(
                employee.Id,
                "Employee updated successfully");
        }

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
            CreatedAt = DateTime.UtcNow,
            TenantId = _tenant.TenantId
        };

        var profile = new UserProfile
        {
            Id = user.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
            Gender = request.Gender,
            DepartmentId = request.DepartmentId,
            JobTitleId = request.JobTitleId,
            ProfileImageUrl = request.ProfileImageUrl,
            TenantId = user.TenantId
        };

        _context.Users.Add(user);
        _context.UserProfile.Add(profile);

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(
            profile.Id,
            "Employee created successfully");
    }
}