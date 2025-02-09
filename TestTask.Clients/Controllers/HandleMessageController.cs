using Microsoft.AspNetCore.Mvc;

namespace TestTask.Clients.Controllers
{
    public class HandleMessageController : Controller
    {
        private readonly ILogger<HandleMessageController> _logger;

        public HandleMessageController(ILogger<HandleMessageController> logger, IConfiguration configuration)
        {
            _logger = logger;

            ViewData.Add("ReceiveMethodName", configuration["SignalR:ReceiveMethodName"]);
            ViewData.Add("Endpoint", $"{configuration["SignalR:Endpoint"]}");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
