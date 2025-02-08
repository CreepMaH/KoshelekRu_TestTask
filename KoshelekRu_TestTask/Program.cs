using KoshelekRu_TestTask.Domain.Interfaces;
using KoshelekRu_TestTask.Repository;
using KoshelekRu_TestTask.Services;
using Scalar.AspNetCore;

namespace KoshelekRu_TestTask
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
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSignalR();
            builder.Services.AddTransient<IMessageDBRepository, MessagePostgreSQL>();
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<MessageHub>(app.Configuration["SignalR:Endpoint"]!);
        }
    }
}
