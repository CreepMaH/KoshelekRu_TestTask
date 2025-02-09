using Microsoft.AspNetCore.SignalR.Client;

namespace TestTask.Clients.Services
{
    public class SignalRClient
    {
        private readonly HubConnection _connection;
        private readonly string _receiveMethodName;

        public SignalRClient(string hubUrl, string receiveMethodName)
        {
            _receiveMethodName = receiveMethodName;

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _connection.On<string, string>(_receiveMethodName, (user, message) =>
            {
                //_logger.LogInformation(message);
            });
        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async Task SendMessageAsync(string user, string message)
        {
            await _connection.InvokeAsync(_receiveMethodName, user, message);
        }
    }
}
