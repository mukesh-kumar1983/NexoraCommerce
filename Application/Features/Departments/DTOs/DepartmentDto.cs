namespace Application.Features.Departments.Dtos;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}