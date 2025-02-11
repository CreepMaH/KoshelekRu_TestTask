using Microsoft.AspNetCore.SignalR;
using System.Text;
using TestTask.Domain.Extensions;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Server.Services
{
    public class MessageHub(ILogger<MessageHub> logger, IConfiguration configuration, IMessageDBRepository dBRepository)
        : Hub
    {
        private readonly string _receiveMethodName = configuration["SignalR:ReceiveMethodName"]!;
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
            _logger.LogTrace("Message received.\r\nUser: {user}. Text: {message}.", user, jsonMessage);

            Message message = jsonMessage.JsonStringToMessage();
            message.TimeStamp = DateTime.Now;

            var result = await _dBRepository.Write(message);
            if (!result.IsSuccess)
            {
                _logger.LogError("An error occured while saving to DB. Text: {errorMessage}", result.Message);
            }

            await Clients.All.SendAsync(_receiveMethodName, user, jsonMessage);
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogTrace("A new client has connected. Connection ID: {connectionId}", Context.ConnectionId);
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

            _logger.LogTrace(message);
            return Task.CompletedTask;
        }
    }
}
