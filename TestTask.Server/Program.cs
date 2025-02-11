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

            builder.Services.AddSingleton<IMessageDBRepositoryBuilder<IMessageDBRepository>, MessagePostgreSQLBuilder>();
            builder.Services.AddSingleton<IMessageDBRepository>(serviceProvider =>
            {
                var messageBuilder = serviceProvider.GetRequiredService<IMessageDBRepositoryBuilder<IMessageDBRepository>>();
                return messageBuilder.Build().GetAwaiter().GetResult();
            });
        }
        private static void ConfigureMiddleware(WebApplication app)
        {
            app.UseCors(options => options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("https://localhost:7103", "https://localhost:7063")
                //.WithOrigins(app.Configuration["SignalR:CorsAllowedOrigins"]!)
                );
            app.UseHttpsRedirection();
            app.MapHub<MessageHub>(app.Configuration["SignalR:Endpoint"]!);
        }
    }
}
