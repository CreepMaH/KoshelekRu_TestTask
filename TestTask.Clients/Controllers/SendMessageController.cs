using Microsoft.AspNetCore.Mvc;
using TestTask.Clients.Services;

namespace TestTask.Clients.Controllers
{
    [Route("[controller]")]
    public class SendMessageController(SignalRClient hubClient) 
        : Controller
    {
        private readonly SignalRClient _hubClient = hubClient;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            await _hubClient.SendMessageAsync(user, message);
            return Ok("The message has been sent.");
        }
    }
}
