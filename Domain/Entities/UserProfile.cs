using Domain.Common;
using Domain.Entities;
using NexoraEnterprise.SharedKernel.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;



public class UserProfile : BaseEntity, ITenantEntity
{
    // ----------------------------------------------------
    // Identity Relationship (PK = FK)
    // ----------------------------------------------------

    [Key]
    public Guid Id { get; set; }

    [JsonIgnore]
    public AppUser User { get; set; } = null!;

    // ----------------------------------------------------
    // Basic Information
    // ----------------------------------------------------

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public string FullName =>
        $"{FirstName} {LastName}".Trim();

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

    // ----------------------------------------------------
    // Employee Information
    // ----------------------------------------------------

    [MaxLength(50)]
    public string? EmployeeNumber { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? ConfirmationDate { get; set; }

    public bool IsEmployee { get; set; } = true;

    public bool IsActiveEmployee { get; set; } = true;

    // ----------------------------------------------------
    // Organization Structure
    // ----------------------------------------------------

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid? JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }

    // Future Organization Chart
    public Guid? ManagerId { get; set; }

    // Self-reference
    public UserProfile? Manager { get; set; }

    // ----------------------------------------------------
    // Tenant Information
    // ----------------------------------------------------

    public Guid TenantId { get; set; }

    public Tenant? Tenant { get; set; }

    // ----------------------------------------------------
    // Profile Image
    // ----------------------------------------------------

    [MaxLength(1000)]
    public string? ProfileImageUrl { get; set; }

    // ----------------------------------------------------
    // Employment Metadata
    // ----------------------------------------------------

    [MaxLength(50)]
    public string? EmploymentType { get; set; }
    // Permanent
    // Contract
    // Consultant
    // Intern

    [MaxLength(50)]
    public string? EmploymentStatus { get; set; }
    // Active
    // On Leave
    // Suspended
    // Resigned
    // Terminated
}