using DLQService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DLQService.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/dlq")]
    public class DlqController : ControllerBase
    {
        private readonly DlqService _service;

        public DlqController(DlqService service)
        {
            _service = service;
        }

        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            var messages = _service.GetMessages();
            return Ok(messages);
        }

        [HttpPost("replay")]
        public IActionResult Replay([FromBody] ReplayRequest request)
        {
            _service.ReplayMessage(request.DeliveryTag, request.Payload);

            return Ok(new
            {
                message = "Message replayed successfully"
            });
        }

        [HttpDelete("delete/{deliveryTag}")]
        public IActionResult Delete(ulong deliveryTag)
        {
            _service.DeleteMessage(deliveryTag);

            return Ok(new
            {
                message = "Message deleted successfully"
            });
        }

    }

    public class ReplayRequest
    {
        public ulong DeliveryTag { get; set; }

        public string Payload { get; set; }
    }
}
