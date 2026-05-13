using AuthService.API.DTOs.Authentication;
using AuthService.Application.Features.Commands;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Register

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequestDto dto)
    {
        var command = new RegisterUserCommand
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password
        }; 

        var result = await _mediator.Send(command);

        

        return Ok(result);
    }

    

    #endregion

    #region Login

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var command = new LoginCommand
        {
            Email = dto.Email,
            Password = dto.Password
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    #endregion

    #region Logout

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(ApiResponse<string>
            .SuccessResponse("Logged out successfully"));
    }

    #endregion
}