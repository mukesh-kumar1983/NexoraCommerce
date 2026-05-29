using AuthService.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Commands.CreateEmployeeCommand
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IAuthDbContext _context;

        public CreateEmployeeCommandHandler(IAuthDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            //var employee = new Employee
            //{
            //    FirstName = request.FirstName,
            //    LastName = request.LastName,
            //    Email = request.Email,
            //    Department = request.Department
            //};

            //_context.Employees.Add(employee);
            //await _context.SaveChangesAsync(cancellationToken);

            //return employee.Id;

            return 0;
        }
    }
}
