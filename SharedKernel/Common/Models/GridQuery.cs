namespace SharedKernel.Common.Models;

/// <summary>
/// Unified request model for Grid + Export
/// </summary>
public class GridQuery
{
    public string? Search { get; set; }

    public string? sortColumn { get; set; }

    public string? sortDirection { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}