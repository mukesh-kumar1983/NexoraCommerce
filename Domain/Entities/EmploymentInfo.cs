using NexoraEnterprise.SharedKernel.Common.Models;

public class EmploymentInfo : BaseEntity
{
    public Guid UserId { get; set; }

    public string? EmployeeNumber { get; set; }
    public DateTime? JoiningDate { get; set; }

    public string? EmploymentType { get; set; }   // Permanent, Contract
    public string? EmploymentStatus { get; set; } // Active, Suspended
}