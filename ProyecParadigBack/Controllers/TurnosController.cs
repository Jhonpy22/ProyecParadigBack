
using Application.Contratos.Turnos;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProyecParadigBack.Hubs;

namespace ProyecParadigBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnosController : ControllerBase
    {
        private readonly ISvTurnos _svTurnos;
        private readonly IHubContext<GameHub> _hub;

        public TurnosController(ISvTurnos svTurnos, IHubContext<GameHub> hub)
        {
            _svTurnos = svTurnos;
            _hub = hub;
        }

        // POST: api/turnos/voltear
        [HttpPost("voltear")]
        public async Task<ActionResult> VoltearCarta([FromBody] VoltearCartaRequest request)
        {
            var partida = await _svTurnos.VoltearAsync(request);

            string salaCodigo = partida.SalaId.ToString();

            
            await _hub.Clients.Group(salaCodigo).SendAsync("GameUpdated", partida);

            
            if (partida.Estado == EstadoPartida.Finalizada)
            {
                await _hub.Clients.Group(salaCodigo).SendAsync("GameEnded", partida);
                await _hub.Clients.Group(salaCodigo).SendAsync("RankingUpdated", new
                {
                    Ranking = partida.Jugadores.OrderByDescending(j => j.Puntaje).ToList()
                });
            }
            

            
            var siguienteJugador = partida.Jugadores.FirstOrDefault(j => j.JugadorId ==partida.JugadorActualId);
            if (siguienteJugador != null)
            {
                await _hub.Clients.Group(salaCodigo)
                    .SendAsync("TurnChanged", siguienteJugador.Nombre);
            }

            return Ok(partida);
        }
    }
}
