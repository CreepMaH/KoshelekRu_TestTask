using TestTask.Domain.Interfaces;
using TestTask.Repository;
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
            builder.Services.AddTransient<IMessageDBRepository, MessagePostgreSQL>();
        }
        private static void ConfigureMiddleware(WebApplication app)
        {
            app.UseCors(options => options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("https://localhost:7103", "https://localhost:7063")
                );
            app.UseHttpsRedirection();
            app.MapHub<MessageHub>(app.Configuration["SignalR:Endpoint"]!);
        }
    }
}
