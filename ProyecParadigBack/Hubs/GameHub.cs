using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace ProyecParadigBack.Hubs
{
    public class GameHub : Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            
        }

        public async Task SendMessage(string roomId, string message)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        
    }
}
