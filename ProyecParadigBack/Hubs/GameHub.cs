using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace ProyecParadigBack.Hubs
{
    public class GameHub : Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("OnConnected", "Conectado al GameHub correctamente.");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("PlayerJoined", Context.ConnectionId);
        }

        public async Task SendMessage(string roomId, string message)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        
    }
}
