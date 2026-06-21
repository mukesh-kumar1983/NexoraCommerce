namespace NexoraEnterprise.SharedKernel.Common.Errors;

/// <summary>
/// Centralized application error codes used across all services.
/// These codes are machine-readable and stable (never change them once published).
/// </summary>
public static class ErrorCodes
{
    // =========================
    // AUTHENTICATION ERRORS
    // =========================
    public const string Auth_InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
    public const string Auth_UserLocked = "AUTH_USER_LOCKED";
    public const string Auth_UserNotFound = "AUTH_USER_NOT_FOUND";
    public const string Auth_Unauthorized = "AUTH_UNAUTHORIZED";

    // =========================
    // TENANT ERRORS (SaaS CORE)
    // =========================
    public const string Tenant_NotFound = "TENANT_NOT_FOUND";
    public const string Tenant_Inactive = "TENANT_INACTIVE";
    public const string Tenant_Mismatch = "TENANT_MISMATCH";

    // =========================
    // VALIDATION ERRORS
    // =========================
    public const string Validation_Failed = "VALIDATION_FAILED";

    // =========================
    // GENERAL ERRORS
    // =========================
    public const string General_NotFound = "RESOURCE_NOT_FOUND";
    public const string General_ServerError = "INTERNAL_SERVER_ERROR";
}