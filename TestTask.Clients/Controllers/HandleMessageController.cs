using Microsoft.AspNetCore.Mvc;

namespace TestTask.Clients.Controllers
{
    public class HandleMessageController(IConfiguration configuration) 
        : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        public IActionResult Index()
        {
            ViewData.Add("ReceiveMethodName", _configuration["SignalR:ReceiveMethodName"]);
            ViewData.Add("ServerHost", $"{_configuration["SignalR:ServerHost"]}");
            ViewData.Add("ServerHostOuter", $"{_configuration["SignalR:ServerHostOuter"]}");
            ViewData.Add("Endpoint", $"{_configuration["SignalR:Endpoint"]}");

            return View();
        }
    }
}
