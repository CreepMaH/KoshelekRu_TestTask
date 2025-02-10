using Microsoft.AspNetCore.Mvc;

namespace TestTask.Clients.Controllers
{
    public class HandleMessageController : Controller
    {
        public HandleMessageController(IConfiguration configuration)
        {
            ViewData.Add("ReceiveMethodName", configuration["SignalR:ReceiveMethodName"]);
            ViewData.Add("Endpoint", $"{configuration["SignalR:Endpoint"]}");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
