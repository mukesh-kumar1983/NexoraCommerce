using EmailService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailService.API.Controllers;

/// <summary>
/// Provides operational endpoints for
/// managing failed email messages.
/// </summary>
/// 
//[Authorize("Role=Admin")]
[ApiController]
[Route("api/dlq")]
public class DlqController : ControllerBase
{
    private readonly IEmailDbContext _context;

    public DlqController(IEmailDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns all failed email messages.
    /// </summary>
    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages()
    {
        var messages = await _context.FailedEmailMessages
            .OrderByDescending(x => x.FailedAt)
            .ToListAsync();

        return Ok(messages);
    }
}