using KoshelekRu_TestTask.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoshelekRu_TestTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        IMessageDBRepository _messageDBRepo;

        public MessagesController(IMessageDBRepository messageDBRepo)
        {
            _messageDBRepo = messageDBRepo;
        }

        [HttpPost("write-message")]
        public async Task<ActionResult> WriteMessage(string message)
        {
            var result = await _messageDBRepo.Write(message);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }


    }
}
