namespace TestTask.Domain.Models.AppSettings
{
    public class SignalRSettings
    {
        public string? ServerHostInDockerNetwork { get; set; }
        public string? ServerHostInPublicNetwork { get; set; }
        public string[]? CorsAllowedOrigins { get; set; }
        public string? Endpoint { get; set; }
        public string? ReceiveMethodName { get; set; }
    }
}
