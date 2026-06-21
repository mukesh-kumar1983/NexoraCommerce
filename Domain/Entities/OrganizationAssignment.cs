using NexoraEnterprise.SharedKernel.Common.Models;

public class OrganizationAssignment : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid? DepartmentId { get; set; }
    public Guid? JobTitleId { get; set; }
    public Guid? ManagerId { get; set; }
}