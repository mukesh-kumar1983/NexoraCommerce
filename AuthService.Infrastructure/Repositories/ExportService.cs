using AuthService.Application.Common.Interfaces;
using ClosedXML.Excel;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.SharedKernel.Common.Enums;
using NexoraEnterprise.SharedKernel.Common.Exports;
using QuestPDF.Fluent;
using System.Reflection;

namespace AuthService.Infrastructure.Repositories;

public class ExportService : IExportService
{
    public ExportFileResult Export<T>(
        IEnumerable<T> data,
        ExportFormat format,
        ExportDefinition? definition = null)
    {
        return format switch
        {
            ExportFormat.Excel => ExportExcel(data, definition),
            ExportFormat.Pdf => ExportPdf(data, definition),
            _ => throw new Exception("Invalid export format")
        };
    }

    #region EXCEL

    private ExportFileResult ExportExcel<T>(
        IEnumerable<T> data,
        ExportDefinition? definition)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add(definition?.SheetName ?? "Sheet1");

        var props = GetProperties<T>(definition);

        // Header
        for (int i = 0; i < props.Count; i++)
        {
            ws.Cell(1, i + 1).Value = props[i].Item2;
        }

        // Data
        int row = 2;

        foreach (var item in data)
        {
            for (int col = 0; col < props.Count; col++)
            {
                var value = props[col].Item1.GetValue(item);
                ws.Cell(row, col + 1).Value = value?.ToString();
            }
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new ExportFileResult
        {
            FileContent = stream.ToArray(),
            FileName = $"export_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };
    }

    #endregion

    #region PDF

    private ExportFileResult ExportPdf<T>(
        IEnumerable<T> data,
        ExportDefinition? definition)
    {
        var props = GetProperties<T>(definition);

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var _ in props)
                            columns.RelativeColumn();
                    });

                    // Header
                    table.Header(header =>
                    {
                        foreach (var prop in props)
                        {
                            header.Cell()
                                .Background("#eee")
                                .Padding(5)
                                .Text(prop.Item2);
                        }
                    });

                    // Rows
                    foreach (var item in data)
                    {
                        foreach (var prop in props)
                        {
                            var value = prop.Item1.GetValue(item);
                            table.Cell()
                                .Padding(5)
                                .Text(value?.ToString() ?? "");
                        }
                    }
                });
            });
        });

        using var stream = new MemoryStream();
        document.GeneratePdf(stream);

        return new ExportFileResult
        {
            FileContent = stream.ToArray(),
            FileName = $"export_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf",
            ContentType = "application/pdf"
        };
    }

    #endregion

    #region HELPERS

    private List<(PropertyInfo, string)> GetProperties<T>(ExportDefinition? definition)
    {
        var props = typeof(T).GetProperties();

        var list = props.Select(p => (p, p.Name)).ToList();

        // If custom definition exists
        if (definition?.Columns?.Any() == true)
        {
            list = definition.Columns
                .Where(c => !c.Ignore)
                .OrderBy(c => c.Order)
                .Select(c =>
                {
                    var prop = props.First(p => p.Name == c.Field);
                    return (prop, c.Header);
                })
                .ToList();
        }

        return list;
    }

    #endregion
}