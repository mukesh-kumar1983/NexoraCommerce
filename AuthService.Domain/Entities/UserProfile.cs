using NexoraEnterprise.AuthService.Domain.Entities;
using NexoraEnterprise.SharedKernel.Common.Models;
using System.Text.Json.Serialization;

namespace NexoraEnterprise.AuthService.Domain;

public class UserProfile : BaseEntity
{
    public Guid Id { get; set; }   // MUST be explicit FK + PK

    [JsonIgnore]
    public AppUser User { get; set; } = null!;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Gender { get; set; }

    public Guid? JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }

    public Guid? TenantId { get; set; }

    public Tenant? Tenant { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public string? ProfileImageUrl { get; set; }
}