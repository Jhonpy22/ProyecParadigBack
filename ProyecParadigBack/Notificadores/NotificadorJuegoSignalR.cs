using Application.Contratos.Partidas;
using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using ProyecParadigBack.Hubs;

namespace ProyecParadigBack.Notificadores
{
    public class NotificadorJuegoSignalR : INotificadorJuego
    {
        private readonly IHubContext<GameHub> _hub;

        public NotificadorJuegoSignalR(IHubContext<GameHub> hub)
        {
            _hub = hub;
        }

        public async Task EnviarPartidaFinalizadaAsync(PartidaDto partida, string motivo)
        {
            var ganador = partida.Jugadores.FirstOrDefault(j => j.JugadorId == partida.GanadorId);

            await _hub.Clients.Group(partida.SalaCodigo)
                .SendAsync("GameEnded", new { 
                    motivo,
                    ganador = ganador?.Nombre ?? "Sin ganador",
                    puntajeGanador = ganador?.Puntaje ?? 0
                });
        }
    }
}
