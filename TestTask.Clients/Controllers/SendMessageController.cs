using Microsoft.AspNetCore.Mvc;
using TestTask.Clients.Services;

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
        /// <param name="message">Message</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            var result =  await _hubClient.SendMessageAsync(user, message);

            string sendingResult = result.IsSuccess 
                ? $"Message \"{message}\" has been successfully sended"
                : $"An error occured while sending. Text: {result.Message}";

            TempData["SendingResult"] = sendingResult;

            return RedirectToAction("Index");
        }
    }
}
