using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.Commands.UpdateUserProfile;
using AuthService.Application.Features.Users.Commands.UploadProfileImage;
using AuthService.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAzureBlobService _blobService;
        private  readonly IUserService _userService;

        public UserController(IMediator mediator, IAzureBlobService blobService, IUserService userService)
        {
            _mediator = mediator;
            _blobService = blobService;
            _userService = userService;
        }

        //[HttpPost("upload-profile-image")]
        //public async Task<IActionResult> UploadProfileImage([FromForm] UploadFileCommand command)
        //{
        //    var imageUrl = await _mediator.Send(command);

        //    return Ok(new { imageUrl });
        //}

        [Authorize]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // upload to Azure
            var imageUrl = await _blobService.UploadFileAsync(file);

            // update DB
            await _userService.UpdateProfileImageAsync(userId, imageUrl);

            return Ok(new { imageUrl });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateMyProfileCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("User not found");

            return Ok("Profile updated successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _mediator.Send(new GetUsersQuery());
            return Ok(employees);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _mediator.Send(new GetMyProfileQuery());
            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("User not found");

            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
    }
}
