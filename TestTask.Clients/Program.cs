using Scalar.AspNetCore;
using TestTask.Clients.Services;
using TestTask.Configuration.Services;
using TestTask.Domain.Interfaces;
using TestTask.Repository.PostgreSQL;

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
            builder.Services.AddOpenApi();

            builder.Services.AddSingleton<IAppSettings, AppSettingsService>();
            builder.Services.AddSingleton<SignalRClient>(serviceProvider =>
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

            builder.Services.AddSingleton<IMessageDBRepositoryBuilder<IMessageDBRepository>, MessagePostgreSQLBuilder>();
            builder.Services.AddScoped<IMessageDBRepository>(serviceProvider =>
            {
                var dbBuilder = serviceProvider.GetRequiredService<IMessageDBRepositoryBuilder<IMessageDBRepository>>();
                return dbBuilder.Build().GetAwaiter().GetResult();
            });
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.MapOpenApi();

            var configs = app.Services.GetRequiredService<IAppSettings>()
                .GetAppSettings();
            app.MapScalarApiReference(options =>
            {
                options.AddServer(new ScalarServer(configs.ScalarSettings!.ClientUrl!));
            });

            app.UseRouting();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
        }
    }
}
