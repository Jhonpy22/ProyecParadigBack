    using Microsoft.AspNetCore.SignalR;


    namespace ProyecParadigBack.Hubs
    {
        public class GameHub : Hub
        {
            private readonly ILogger<GameHub> _logger;

            public GameHub(ILogger<GameHub> logger)
            {
                _logger = logger;
            }
            public override async Task OnConnectedAsync()
            {
                _logger.LogInformation(" Cliente conectado: {ConnectionId}", Context.ConnectionId);
                await base.OnConnectedAsync();
            
            }

            public override async Task OnDisconnectedAsync(Exception? exception)
            {
                _logger.LogInformation(" Cliente desconectado: {ConnectionId}", Context.ConnectionId);
                if (exception != null)
                {
                    _logger.LogWarning(exception, " Cliente desconectado con excepción: {ConnectionId}", Context.ConnectionId);
                }
                await base.OnDisconnectedAsync(exception);
            }

            public async Task JoinRoom(string roomId)
            {
                try
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                    _logger.LogInformation("Cliente {ConnectionId} se unió al grupo '{RoomId}'", Context.ConnectionId, roomId);

                    await Clients.OthersInGroup(roomId).SendAsync("PlayerConnected", Context.ConnectionId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uniendo cliente {ConnectionId} al grupo '{RoomId}'", Context.ConnectionId, roomId);
                
                    await Clients.Caller.SendAsync("Error", "No se pudo unir a la sala");
                }
            }

            public async Task LeaveRoom(string roomId)
            {
                try
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
                    _logger.LogInformation("Cliente {ConnectionId} salió del grupo '{RoomId}'", Context.ConnectionId, roomId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removiendo cliente {ConnectionId} del grupo '{RoomId}'", Context.ConnectionId, roomId);
               
                }
            }

            public async Task SendMessage(string roomId, string message)
            {
                try {
                    await Clients.Group(roomId).SendAsync("ReceiveMessage", Context.ConnectionId, message);
                    _logger.LogDebug("Mensaje enviado de {ConnectionId} al grupo '{RoomId}': {Message}", Context.ConnectionId, roomId, message);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enviando mensaje de {ConnectionId} al grupo '{RoomId}'", Context.ConnectionId, roomId);
                    await Clients.Caller.SendAsync("Error", "No se pudo enviar el mensaje");
                }
            }
        
        }
    }
