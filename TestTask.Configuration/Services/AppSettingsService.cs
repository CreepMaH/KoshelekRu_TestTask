using System.Text.Json;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models.AppSettings;

namespace TestTask.Configuration.Services
{
    public class AppSettingsService : IAppSettings
    {
        private readonly string _appSettingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private static AppSettings? _appSettings;

        public AppSettings GetAppSettings()
        {
            _appSettings ??= ReadAppSettingsFromFile(_appSettingFilePath) 
                ?? throw new ArgumentNullException(nameof(AppSettings));
            return _appSettings;
        }

        private AppSettings? ReadAppSettingsFromFile(string filePath)
        {
            string text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<AppSettings>(text);
        }
    }
}
