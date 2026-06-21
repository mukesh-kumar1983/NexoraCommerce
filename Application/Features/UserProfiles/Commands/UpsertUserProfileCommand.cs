using MediatR;

namespace Application.Features.UserProfiles.Commands.UpsertUserProfile;

/// <summary>
/// Creates or updates user profile for current logged-in user.
/// </summary>
public record UpsertUserProfileCommand(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Address,
    string? City,
    string? Country,
    string? Gender,
    DateTime? DateOfBirth,
    Guid? DepartmentId,
    Guid? JobTitleId,
    string? ProfileImageUrl
) : IRequest<ApiResponse<Guid>>;