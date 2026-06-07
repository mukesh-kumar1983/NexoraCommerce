
namespace NexoraEnterprise.SharedKernel.Models
{

}
public class PagedRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    // Multi-column sorting ready (future-proof)
    public string? SortColumn { get; set; }

    public string SortDirection { get; set; } = "asc";

    // Future: filtering extension point
    public Dictionary<string, string>? Filters { get; set; }
}