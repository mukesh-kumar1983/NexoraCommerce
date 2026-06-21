namespace Application.Features.JobTitles.Queries.Models;

public class JobTitleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public  Guid TenantId { get; set; }
}