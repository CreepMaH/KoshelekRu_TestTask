using TestTask.Clients.Services;
using TestTask.Domain.Interfaces;
using TestTask.Repository;

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
            builder.Services.AddTransient<IMessageDBRepository, MessagePostgreSQL>();
            builder.Services.AddSingleton(service =>
            {
                string hubEndpoint = builder.Configuration["SignalR:Endpoint"]!;
                string hubUrl = $"https://localhost:7063{hubEndpoint}";
                string receiveMethodName = builder.Configuration["SignalR:ReceiveMethodName"]!;
                var client = new SignalRClient(hubUrl, receiveMethodName);
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
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=HandleMessage}/{action=Index}/{id?}")
                .WithStaticAssets();
        }
    }
}
