using NexoraEnterprise.SharedKernel.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

/// <summary>
/// Business profile of system user (HR layer).
/// Separate from authentication (AppUser).
/// </summary>
public class UserProfile : BaseEntity
{
    // Identity (1:1 with AppUser)
    public Guid UserId { get; set; }

    [JsonIgnore]
    public AppUser User { get; set; } = null;

    // Basic Info
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(20)]
    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    // Employment Info
    [MaxLength(50)]
    public string? EmployeeNumber { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? ConfirmationDate { get; set; }

    public bool IsEmployee { get; set; } = true;

    public bool IsActiveEmployee { get; set; } = true;

    [MaxLength(50)]
    public string? EmploymentType { get; set; }

    [MaxLength(50)]
    public string? EmploymentStatus { get; set; }

    // Org Structure
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid? JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }

    public Guid? ManagerId { get; set; }
    public UserProfile? Manager { get; set; }

    // Tenant
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    // Profile Image
    [MaxLength(1000)]
    public string? ProfileImageUrl { get; set; }
}