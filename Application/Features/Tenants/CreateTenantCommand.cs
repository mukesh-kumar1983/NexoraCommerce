using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Features.Tenants
{
    public class CreateTenantCommand : IRequest<ApiResponse<Guid>>
    {
        public string Name { get; set; } = default!;
        public string Subdomain { get; set; } = default!;

        public string AdminEmail { get; set; }= default!;

        public string AdminPassword { get; set; }= default!;
    }
}
