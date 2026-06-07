using NexoraEnterprise.SharedKernel.Common.Enums;
using NexoraEnterprise.SharedKernel.Common.Exports;

namespace NexoraEnterprise.AuthService.Application.Common.Interfaces;

public interface IExportService
{
    ExportFileResult Export<T>(
        IEnumerable<T> data,
        ExportFormat format,
        ExportDefinition? definition = null);
}