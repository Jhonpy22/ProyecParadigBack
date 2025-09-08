using Application.Contratos.Partidas;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProyecParadigBack.Hubs;

namespace ProyecParadigBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasController : ControllerBase
    {
        private readonly ISvPartidas _svPartidas;
        private readonly IHubContext<GameHub> _hub;

        public PartidasController(ISvPartidas svPartidas, IHubContext<GameHub> hub)
        {
            _svPartidas = svPartidas;
            _hub = hub;
        }

        // POST: api/partidas/iniciar
        [HttpPost("iniciar")]
        public async Task<ActionResult<PartidaDto>> IniciarPartida([FromBody] IniciarPartidaRequest request)
        {
            var partida = await _svPartidas.IniciarAsync(request);

            await _hub.Clients.Group(partida.SalaId.ToString())
                .SendAsync("GameUpdated", partida);

            return CreatedAtAction(nameof(ObtenerPartida), new { partidaId = partida.PartidaId }, partida);
        }

        // GET: api/partidas/{partidaId}
        [HttpGet("{partidaId}")]
        public async Task<ActionResult<PartidaDto>> ObtenerPartida(int partidaId)
        {
            var partida = await _svPartidas.ObtenerAsync(partidaId);
            return Ok(partida);
        }
    }
}
