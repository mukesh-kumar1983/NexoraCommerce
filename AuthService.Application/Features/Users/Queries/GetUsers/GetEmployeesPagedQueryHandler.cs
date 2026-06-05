using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Querying;
using AuthService.Application.Features.Users.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

public class GetEmployeesPagedQueryHandler
    : IRequestHandler<GetEmployeesPagedQuery, PagedResult<EmployeeDto>>
{
    private readonly IAuthDbContext _context;

    public GetEmployeesPagedQueryHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<EmployeeDto>> Handle(
        GetEmployeesPagedQuery request,
        CancellationToken cancellationToken)
    {
        // =====================================
        // BASE QUERY
        // =====================================
        var query =
            from u in _context.Users
            join p in _context.UserProfile on u.Id equals p.Id
            join d in _context.Department on p.DepartmentId equals d.Id
            join j in _context.JobTitle on p.JobTitleId equals j.Id
            where !u.IsDeleted
            select new EmployeeDto
            {
                Id = u.Id,
                FirstName = p.FirstName ?? "",
                LastName = p.LastName ?? "",
                Email = u.Email,
                DepartmentName = d.Title,
                JobTitleName = j.Title,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                City = p.City,
                Country = p.Country
            };

        // =====================================
        // SEARCH
        // =====================================
        query = query.ApplySearch(
            request.Search,

            x => x.FirstName,
            x => x.LastName,
            x => x.Email,
            x => x.DepartmentName,
            x => x.JobTitleName,
            x => x.PhoneNumber ?? "",
            x => x.Address ?? "",
            x => x.City ?? "",
            x => x.Country ?? ""
        );

        // =====================================
        // SORT MAP
        // =====================================
        var sortMap =
            new Dictionary<string, System.Linq.Expressions.Expression<Func<EmployeeDto, object>>>
            {
                ["firstname"] = x => x.FirstName,
                ["lastname"] = x => x.LastName,
                ["email"] = x => x.Email,
                ["departmentname"] = x => x.DepartmentName,
                ["jobtitlename"] = x => x.JobTitleName,
                ["phonenumber"] = x => x.PhoneNumber!,
                ["address"] = x => x.Address!,
                ["city"] = x => x.City!,
                ["country"] = x => x.Country!
            };

        // =====================================
        // SORTING
        // =====================================
        query = query.ApplySorting(
            request.sortColumn,
            request.sortDirection,
            sortMap);

        // =====================================
        // COUNT
        // =====================================
        var totalCount = await query.CountAsync(cancellationToken);

        // =====================================
        // PAGING
        // =====================================
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<EmployeeDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}