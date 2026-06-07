namespace NexoraEnterprise.AuthService.Application.Features.Users.Commands.ExportEmployees
{
    using MediatR;
    using NexoraEnterprise.SharedKernel.Common.Enums;
    using NexoraEnterprise.SharedKernel.Common.Exports;

    public class ExportEmployeesCommand : IRequest<ExportFileResult>
    {
        public ExportFormat Format { get; set; }

        public ExportRequest Request { get; set; } = default!;

        // ✅ ADD THIS
        public ExportDefinition? Definition { get; set; }
    }
}
