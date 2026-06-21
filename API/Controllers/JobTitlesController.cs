using Application.Features.JobTitles.Commands.CreateJobTitle;
using Application.Features.JobTitles.Commands.DeleteJobTitle;
using Application.Features.JobTitles.Commands.UpdateJobTitle;
using Application.Features.JobTitles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/jobtitles")]
public class JobTitlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobTitlesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetJobTitlesQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetJobTitleByIdQuery(id));

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobTitleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateJobTitleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteJobTitleCommand
        {
            Id = id
        });

        return Ok(result);
    }
}