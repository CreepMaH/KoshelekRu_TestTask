namespace TestTask.Domain.Models.AppSettings
{
    public class AppSettings
    {
        public SignalRSettings? SignalRSettings { get; set; }
        public DBConfig? DBConfig { get; set; }
        public ScalarSettings? ScalarSettings { get; set; }
    }
}
