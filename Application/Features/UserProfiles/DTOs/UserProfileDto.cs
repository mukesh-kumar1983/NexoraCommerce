namespace Application.Features.UserProfiles.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim() ?? string.Empty;

    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    public string? Department { get; set; }
    public string? JobTitle { get; set; }

    public string? ProfileImageUrl { get; set; }
}