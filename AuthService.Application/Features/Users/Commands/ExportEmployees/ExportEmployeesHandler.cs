using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.Exports;

namespace AuthService.Application.Features.Users.Commands.ExportEmployees
{
    public class ExportEmployeesHandler
        : IRequestHandler<ExportEmployeesCommand, ExportFileResult>
    {
        private readonly IExportService _exportService;
        private readonly IAuthDbContext _context;

        public ExportEmployeesHandler(
            IExportService exportService,
            IAuthDbContext context)
        {
            _exportService = exportService;
            _context = context;
        }

        public async Task<ExportFileResult> Handle(
            ExportEmployeesCommand request,
            CancellationToken cancellationToken)
        {
            // =========================
            // BASE QUERY (same as Grid)
            // =========================
            var query =
                from u in _context.Users
                join p in _context.UserProfile on u.Id equals p.Id
                join d in _context.Department on p.DepartmentId equals d.Id
                join j in _context.JobTitle on p.JobTitleId equals j.Id
                where !u.IsDeleted
                select new EmployeeReportDto
                {
                    FirstName = p.FirstName!,
                    LastName = p.LastName!,
                    Email = u.Email,
                    DepartmentName = d.Title,
                    JobTitleName = j.Title,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    City = p.City,
                    Country = p.Country
                };

            // =========================
            // APPLY GRID LOGIC (REUSABLE)
            // =========================
            query = query
            .ApplySearch(request.Request.Search)
            .ApplySorting(
            request.Request.SortField,
            request.Request.SortDir);

            // =========================
            // EXECUTE QUERY (NO PAGING IN EXPORT)
            // =========================
            var data = await query
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // =========================
            // EXPORT ENGINE (Excel / PDF)
            // =========================
            return _exportService.Export(
                data,
                request.Format,
                request.Definition);
        }
    }
}