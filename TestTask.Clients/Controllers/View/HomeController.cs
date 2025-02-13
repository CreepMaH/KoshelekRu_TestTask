using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Interfaces;

namespace TestTask.Clients.Controllers.View
{
    public class HomeController(IAppSettings appSettings)
        : Controller
    {
        private readonly IAppSettings _configuration = appSettings;

        public ActionResult Index()
        {
            var configs = _configuration.GetAppSettings();

            ViewData.Add("ScalarApiExplorerUiUrl", $@"{configs.ScalarSettings!.ClientUrl}/scalar");
            ViewData.Add("MessageReceiverUrl", $@"{configs.ScalarSettings!.ClientUrl}/HandleMessage");

            return View();
        }
    }
}
