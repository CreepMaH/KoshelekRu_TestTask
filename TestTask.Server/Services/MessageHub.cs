using Microsoft.AspNetCore.SignalR;
using System.Text;
using TestTask.Domain.Extensions;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Server.Services
{
    /// <summary>
    /// Provides methods to work with SignalR hub.
    /// </summary>
    /// <param name="logger">Instance for logging</param>
    /// <param name="configuration">App settings with URLs</param>
    /// <param name="dBRepository">Instance to work with DB repository</param>
    public class MessageHub(ILogger<MessageHub> logger, IAppSettings configuration, IMessageDBRepository dBRepository)
        : Hub
    {
        private readonly string _receiveMethodName = configuration.GetAppSettings()
            .SignalRSettings!
            .ReceiveMethodName!;
        private readonly ILogger<MessageHub> _logger = logger;
        private readonly IMessageDBRepository _dBRepository = dBRepository;

        /// <summary>
        /// A message handler. Saves message to DB and resends it to clients.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="jsonMessage">Message</param>
        /// <returns></returns>
        public async Task HandleMessage(string user, string jsonMessage)
        {
            _logger.LogInformation("Message received.\r\nUser: {user}. Text: {message}.", user, jsonMessage);

            Message message = jsonMessage.JsonStringToMessage();
            message.TimeStamp = DateTime.Now;

            var result = await _dBRepository.Write(message);
            if (result.IsSuccess)
            {
                _logger.LogInformation("Message {IndexNumber} has been saved in DB.", message.IndexNumber);
            }
            else
            {
                _logger.LogError("An error occured while saving to DB. Text: {errorMessage}", result.Message);
            }

            await Clients.All.SendAsync(_receiveMethodName, user, message.ToJsonString());
            _logger.LogInformation("Message {IndexNumber} has been sent to clients.", message.IndexNumber);
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("A new client has connected. Connection ID: {connectionId}", Context.ConnectionId);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"A client has disconnected");
            if (exception != null)
            {
                stringBuilder.Append($" with an exception. Text: {exception.Message}");
            }

            stringBuilder.Append($". Connection ID: {Context.ConnectionId}.");

            string message = stringBuilder.ToString();
            _logger.LogInformation(message);

            return Task.CompletedTask;
        }
    }
}
