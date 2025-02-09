using Microsoft.AspNetCore.SignalR;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Services
{
    public class MessageHub : Hub
    {
        private readonly string _receiveMethodName;
        private readonly ILogger<MessageHub> _logger;
        private readonly IMessageDBRepository _dBRepository;

        public MessageHub(ILogger<MessageHub> logger, IConfiguration configuration, IMessageDBRepository dBRepository)
        {
            _logger = logger;
            _receiveMethodName = configuration["SignalR:ReceiveMethodName"]!;
            _dBRepository = dBRepository;
        }

        public async Task Receive(string user, string message)
        {
            //Залоггировать
            var result = await _dBRepository.Write(message);
            await Clients.All.SendAsync(_receiveMethodName, user, message);
        }

        public override async Task OnConnectedAsync()
        {
            //Залоггировать
            await Clients.All.SendAsync(_receiveMethodName, "Сonnected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //Залоггировать
            await Clients.All.SendAsync(_receiveMethodName, "Disconnected");
        }
    }
}
