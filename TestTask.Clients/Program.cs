using TestTask.Clients.Services;

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
            builder.Services.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<SignalRClient>>();

                string hubEndpoint = builder.Configuration["SignalR:Endpoint"]!;
                string hubHost = builder.Configuration["SignalR:ServerHost"]!;
                string hubUrl = $"{hubHost}{hubEndpoint}";

                string receiveMethodName = builder.Configuration["SignalR:ReceiveMethodName"]!;

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

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=HandleMessage}/{action=Index}/{id?}")
                .WithStaticAssets();
        }
    }
}
