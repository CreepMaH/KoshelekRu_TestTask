using Microsoft.AspNetCore.SignalR.Client;

namespace TestTask.Clients.Services
{
    public class SignalRClient(ILogger<SignalRClient> logger, string hubUrl, string receiveMethodName)
    {
        private readonly ILogger<SignalRClient> _logger = logger;
        private readonly HubConnection _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
        private readonly string _receiveMethodName = receiveMethodName;

        /// <summary>
        /// Establishes connection with a SignalR hub.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            await _connection.StartAsync();
            _logger.LogInformation("The SignalR client has been started.");
        }

        /// <summary>
        /// Sends a message via connection to the SignalR hub. Doesn't wait for response.
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="message">Message</param>
        /// <returns></returns>
        public async Task SendMessageAsync(string user, string message)
        {
            await _connection.SendAsync(_receiveMethodName, user, message);
            _logger.LogTrace("Message has been sent.\r\nUser {user}. Message: {message}", user, message);
        }
    }
}
