using Application.Contratos.Partidas;
using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using ProyecParadigBack.Hubs;

namespace ProyecParadigBack.Notificadores
{
    public class NotificadorJuegoSignalR : INotificadorJuego
    {
        private readonly IHubContext<GameHub> _hub;
        private readonly ILogger<NotificadorJuegoSignalR> _logger;

        public NotificadorJuegoSignalR(IHubContext<GameHub> hub, ILogger<NotificadorJuegoSignalR> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task EnviarPartidaFinalizadaAsync(PartidaDto partida, string motivo)
        {
            try
            {
                var ganador = partida.Jugadores.FirstOrDefault(j => j.JugadorId == partida.GanadorId);

                await _hub.Clients.Group(partida.SalaCodigo)
                    .SendAsync("GameEnded", new
                    {
                        motivo,
                        ganador = ganador?.Nombre ?? "Sin ganador",
                        puntajeGanador = ganador?.Puntaje ?? 0,
                        partidaId = partida.PartidaId,
                        
                        redirectToLobby = true
                    });

                _logger.LogInformation("Notificación enviada - GameEnded para sala:{SalaCodigo}", partida.SalaCodigo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación GameEnded - Sala:{SalaCodigo}", partida.SalaCodigo);
                throw;
            }
        }
    }
    
}
