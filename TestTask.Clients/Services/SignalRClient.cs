using Microsoft.AspNetCore.SignalR.Client;
using TestTask.Domain.Models;

namespace TestTask.Clients.Services
{
    /// <summary>
    /// Provides methods to communicate with SignalR hub.
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="hubUrl">SignalR hub URL</param>
    /// <param name="receiveMethodName">Hub method to handle messages</param>
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
        /// <param name="jsonMessage">Message</param>
        /// <returns></returns>
        public async Task<OperationResult> SendMessageAsync(string user, string jsonMessage)
        {
            if (_connection.State != HubConnectionState.Connected)
            {
                string errorMessage = $"The connection isn't alive: the state is \"{_connection.State}\". Operation has been aborted.";
                _logger.LogError(errorMessage);

                return new OperationResult
                {
                    IsSuccess = false,
                    Message = errorMessage
                };
            }

            await _connection.SendAsync(_receiveMethodName, user, jsonMessage);
            _logger.LogTrace("Message has been sent.\r\nUser {user}. Message: {message}", user, jsonMessage);

            return new OperationResult
            {
                IsSuccess = true
            };
        }
    }
}
