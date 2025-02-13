using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Interfaces;

namespace TestTask.Clients.Controllers.View
{
    [Route("[controller]")]
    public class HandleMessageController(IAppSettings appSettings)
        : Controller
    {
        private readonly IAppSettings _configuration = appSettings;

        /// <summary>
        /// Show View establishing a websocket connection with SignalR hub and getting messages in real-time.
        /// </summary>
        /// <returns></returns>
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
