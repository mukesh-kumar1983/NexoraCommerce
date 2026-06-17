using Domain.Common;
using NexoraEnterprise.SharedKernel.Common.Models;

public class Employee : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid? JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }

    public DateTime JoiningDate { get; set; }

    public decimal? Salary { get; set; }

    public string Gender { get; set; } = default!;

    public string? ProfileImageUrl { get; set; }

    public bool IsActiveEmployee { get; set; } = true;
}