using MediatR;

public class UpdateMyProfileCommand : IRequest<bool>
{
    //public Guid UserId { get; set; }   // 🔥 REQUIRED (for handler)

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public Guid? DepartmentId { get; set; }
    public Guid? JobTitleId { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Gender { get; set; }

    public string? ProfileImageUrl { get; set; }
}