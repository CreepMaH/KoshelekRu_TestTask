using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestTask.Clients.Services;
using TestTask.Domain.Extensions;
using TestTask.Domain.Models;

namespace TestTask.Clients.Controllers.API
{
    /// <summary>
    /// Provides method to send message to the service via SignalR client.
    /// </summary>
    /// <param name="hubClient">SignalR client object</param>
    [Route("[controller]")]
    [ApiController]
    public class SendMessageController(SignalRClient hubClient)
        : Controller
    {
        private static ulong messagesCount = 0;
        private readonly SignalRClient _hubClient = hubClient;

        /// <summary>
        /// Sends a message to message service. Redirects to an original view with a sending result.
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="messageText">Message</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(string user, string messageText, 
            [FromServices] IValidator<Message> validator)
        {
            var message = new Message
            {
                Text = messageText,
                IndexNumber = ++messagesCount
            };

            var validationResult = validator.Validate(message);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            string jsonMessage = message.ToJsonString();

            var result = await _hubClient.SendMessageAsync(user, jsonMessage);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
