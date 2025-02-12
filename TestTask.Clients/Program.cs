using TestTask.Clients.Services;
using TestTask.Configuration.Services;
using TestTask.Domain.Interfaces;

namespace TestTask.Clients
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AddServices(builder);

            var app = builder.Build();
            ConfigureMiddleware(app);

            app.Run();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IAppSettings, AppSettingsService>();
            builder.Services.AddSingleton(serviceProvider =>
            {
                var configs = serviceProvider.GetRequiredService<IAppSettings>()
                    .GetAppSettings();

                string hubEndpoint = configs.SignalRSettings!.Endpoint!;
                string hubHost = configs.SignalRSettings!.ServerHostInDockerNetwork!;
                string hubUrl = $"{hubHost}{hubEndpoint}";

                string receiveMethodName = configs.SignalRSettings!.ReceiveMethodName!;

                var logger = serviceProvider.GetRequiredService<ILogger<SignalRClient>>();
                var client = new SignalRClient(logger, hubUrl, receiveMethodName);
                client.StartAsync().GetAwaiter().GetResult();

                return client;
            });
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=HandleMessage}/{action=Index}/{id?}")
                .WithStaticAssets();
        }
    }
}
