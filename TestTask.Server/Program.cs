using TestTask.Configuration.Services;
using TestTask.Domain.Interfaces;
using TestTask.Repository.PostgreSQL;
using TestTask.Server.Services;

namespace TestTask.Server
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
            builder.Services.AddCors();
            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IAppSettings, AppSettingsService>();
            builder.Services.AddSingleton<IMessageDBRepositoryBuilder<IMessageDBRepository>, MessagePostgreSQLBuilder>();
            builder.Services.AddScoped<IMessageDBRepository>(serviceProvider =>
            {
                var dbBuilder = serviceProvider.GetRequiredService<IMessageDBRepositoryBuilder<IMessageDBRepository>>();
                return dbBuilder.Build().GetAwaiter().GetResult();
            });
        }
        private static void ConfigureMiddleware(WebApplication app)
        {
            var configs = app.Services.GetRequiredService<IAppSettings>()
                .GetAppSettings();

            string[] allowedOrigins = configs.SignalRSettings!.CorsAllowedOrigins!;

            app.UseCors(options => options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(allowedOrigins)
                );

            app.MapHub<MessageHub>(configs.SignalRSettings!.Endpoint!);
        }
    }
}
