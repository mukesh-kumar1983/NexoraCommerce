using AuthService.Application.Features.Users.Commands.UpdateUserProfile;
using AuthService.Application.Features.Users.Commands.UploadProfileImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage([FromForm] UploadFileCommand command)
        {
            var imageUrl = await _mediator.Send(command);

            return Ok(new { imageUrl });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("User not found");

            return Ok("Profile updated successfully");
        }
    }
}
