using TestTask.Domain.Models.AppSettings;

namespace TestTask.Domain.Interfaces
{
    public interface IAppSettings
    {
        AppSettings GetAppSettings();
    }
}
