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
        private readonly ILogger<PartidasController> _logger;

        public PartidasController(ISvPartidas svPartidas, IHubContext<GameHub> hub, ILogger<PartidasController> logger)
        {
            _svPartidas = svPartidas;
            _hub = hub;
            _logger = logger;
        }

        // POST: api/partidas/iniciar
        [HttpPost("iniciar")]
        public async Task<ActionResult<PartidaDto>> IniciarPartida([FromBody] IniciarPartidaRequest request)
        {
            _logger.LogInformation(" Iniciando partida para SalaId={SalaId}, Dificultad={Dificultad}",
                   request.SalaId, request.Dificultad);

            var partida = await _svPartidas.IniciarAsync(request);

            _logger.LogInformation("Partida creada - ID:{PartidaId}, SalaCodigo:{SalaCodigo}, Jugadores:{JugadoresCount}",
                    partida.PartidaId, partida.SalaCodigo, partida.Jugadores.Count);

          
            await Task.Delay(500);

            await _hub.Clients.Group(partida.SalaCodigo)
                .SendAsync("GameUpdated", new
                {
                    partidaId = partida.PartidaId,
                    dificultad = partida.Dificultad,
                    message = "¡La partida ha comenzado!"
                });

            _logger.LogInformation(" Evento GameUpdated enviado a grupo '{SalaCodigo}' para {JugadoresCount} jugadores",
                partida.SalaCodigo, partida.Jugadores.Count);


            await _hub.Clients.Group(partida.SalaCodigo)
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
