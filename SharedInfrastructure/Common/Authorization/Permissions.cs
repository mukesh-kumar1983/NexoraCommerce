namespace NexoraEnterprise.SharedKernel.Common.Authorization;

/// <summary>
/// Centralized permission definitions (SaaS-wide).
/// </summary>
public static class Permissions
{
    // Employee Module
    public const string Employee_Read = "employee.read";
    public const string Employee_Create = "employee.create";
    public const string Employee_Update = "employee.update";
    public const string Employee_Delete = "employee.delete";

    // Department Module
    public const string Department_Read = "department.read";
    public const string Department_Create = "department.create";

    // Tenant Module (Super Admin only)
    public const string Tenant_Read = "tenant.read";
    public const string Tenant_Create = "tenant.create";
}