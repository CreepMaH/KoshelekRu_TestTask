using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TestTask.Clients.Services;
using TestTask.Domain.Interfaces;

namespace TestTask.Clients.Controllers
{
    [Route("[controller]")]
    public class SendMessageController : Controller
    {
        private readonly IMessageDBRepository _messageDBRepo;
        private readonly SignalRClient _hubClient;
        private readonly string _receiveMethodName;

        public SendMessageController(IMessageDBRepository messageDBRepo,
            SignalRClient hubClient, IConfiguration configuration)
        {
            _messageDBRepo = messageDBRepo;
            _hubClient = hubClient;
            _receiveMethodName = configuration["SignalR:ReceiveMethodName"]!;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteMessage(string user, string message)
        {
            var result = await _messageDBRepo.Write(message);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            await _hubClient.SendMessageAsync(user, message);

            return Ok(result.Message);
        }
    }
}
