using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Lookup.JobTitles.Commands;
using AuthService.Domain.Entities;
using MediatR;

public class CreateJobTitleHandler : IRequestHandler<CreateJobTitleCommand, Guid>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;
    private readonly ICurrentUserService _user;

    public CreateJobTitleHandler(IAuthDbContext context,
    ICurrentTenantService tenant,
    ICurrentUserService user)
    {
        _context = context;
        _tenant = tenant;
        _user = user;
    }

    public async Task<Guid> Handle(CreateJobTitleCommand request, CancellationToken cancellationToken)
    {
        var entity = new JobTitle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            CreatedBy = _user.UserId.ToString(),   
            TenantId = _tenant.TenantId
        };

        _context.JobTitle.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}