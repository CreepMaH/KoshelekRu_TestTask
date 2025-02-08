using Microsoft.AspNetCore.SignalR;

namespace KoshelekRu_TestTask.Services
{
    public class MessageHub : Hub
    {
        private readonly string _receiveMethodName;

        public MessageHub(IConfiguration configuration)
        {
            _receiveMethodName = configuration["SignalR:ReceiveMethodName"]!;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync(_receiveMethodName, $"User with connection {Context.ConnectionId} has joined.");
        }

        public async Task Send(string message)
        {
            await Clients.All.SendAsync(_receiveMethodName, message);
        }
    }
}
