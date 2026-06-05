using SharedKernel.Common.Enums;
using SharedKernel.Common.Exports;
using SharedKernel.Common;

namespace AuthService.Application.Common.Interfaces;

public interface IExportService
{
    ExportFileResult Export<T>(
        IEnumerable<T> data,
        ExportFormat format,
        ExportDefinition? definition = null);
}