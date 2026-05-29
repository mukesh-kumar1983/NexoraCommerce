using AuthService.Application.Features.Lookup.Departments.Commands;
using AuthService.Application.Features.Lookup.Departments.Queries;
using AuthService.Application.Features.Lookup.JobTitles.Commands;
using AuthService.Application.Features.Lookup.JobTitles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Authorize]
    [Route("api/lookups")]
    public class LookupController : ControllerBase
    {

        private readonly IMediator _mediator;

        public LookupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("jobtitles")]
        public async Task<IActionResult> GetJobTitles()
    => Ok(await _mediator.Send(new GetJobTitlesQuery()));

        [HttpPost("jobtitles")]
        public async Task<IActionResult> CreateJobTitle(CreateJobTitleCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPost("departments")]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            return Ok(await _mediator.Send(new GetDepartmentsQuery()));
        }



    }
}
