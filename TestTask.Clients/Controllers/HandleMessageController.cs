using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Interfaces;

namespace TestTask.Clients.Controllers
{
    public class HandleMessageController(IAppSettings appSettings) 
        : Controller
    {
        private readonly IAppSettings _configuration = appSettings;

        public IActionResult Index()
        {
            var configs = _configuration.GetAppSettings();

            ViewData.Add("ReceiveMethodName", configs.SignalRSettings!.ReceiveMethodName);
            ViewData.Add("ServerHost", configs.SignalRSettings.ServerHostInDockerNetwork);
            ViewData.Add("ServerHostOuter", configs.SignalRSettings.ServerHostInPublicNetwork);
            ViewData.Add("Endpoint", configs.SignalRSettings.Endpoint);

            return View();
        }
    }
}
