using System;

namespace Application.Common.Interfaces;

/// <summary>
/// Provides information about the currently authenticated user.
/// Keeps Application layer free from ASP.NET dependencies.
/// </summary>
public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid TenantId { get; }
    string? Email { get; }
}