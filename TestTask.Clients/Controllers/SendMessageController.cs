using Microsoft.AspNetCore.Mvc;
using TestTask.Clients.Services;
using TestTask.Domain.Extensions;
using TestTask.Domain.Models;

namespace TestTask.Clients.Controllers
{
    /// <summary>
    /// Provides method to send message to the service via SignalR client.
    /// </summary>
    /// <param name="hubClient">SignalR client object</param>
    [Route("[controller]")]
    public class SendMessageController(SignalRClient hubClient) 
        : Controller
    {
        private static ulong messagesCount = 0;
        private readonly SignalRClient _hubClient = hubClient;

        /// <summary>
        /// Shows a view to send a message to message service.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Sends a message to message service. Redirects to an original view with a sending result.
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="messageText">Message</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(string user, string messageText)
        {
            string jsonMessage = new Message
            {
                Text = messageText,
                IndexNumber = ++messagesCount
            }.ToJsonString();

            var result =  await _hubClient.SendMessageAsync(user, jsonMessage);

            string sendingResult = result.IsSuccess 
                ? $"Message \"{messageText}\" has been successfully sended"
                : $"An error occured while sending. Text: {result.Message}";

            TempData["SendingResult"] = sendingResult;

            return RedirectToAction("Index");
        }
    }
}
