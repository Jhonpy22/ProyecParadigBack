using Application.Common.Exceptions;
using Application.Contratos.Lobby;
using Application.Contratos.Salas;
using Application.Interfaces;
using Application.Mapeos; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProyecParadigBack.Hubs;
using Microsoft.EntityFrameworkCore;

namespace ProyecParadigBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private readonly ISvSalas _svSalas;
        private readonly IHubContext<GameHub> _hub;
        private readonly AppDbContext _db;

        public SalasController(ISvSalas svSalas, IHubContext<GameHub> hub, AppDbContext db)
        {
            _svSalas = svSalas;
            _hub = hub;
            _db = db;
        }

        // POST: api/salas
        [HttpPost]
        public async Task<ActionResult<SalaDto>> CrearSala([FromBody] CrearSalaRequest request)
        {
            var sala = await _svSalas.CrearAsync(request);
            return CreatedAtAction(nameof(ObtenerSala), new { salaId = sala.SalaId }, sala);
        }

        // POST: api/salas/unirse
        [HttpPost("unirse")]
        public async Task<ActionResult<SalaDto>> UnirseASala([FromBody] UnirseSalaRequest request)
        {
            var sala = await _svSalas.UnirseAsync(request);

            
            await _hub.Clients.Group(sala.CodigoIngreso).SendAsync("PlayerJoined", request.NombreJugador);

            return Ok(sala);
        }

        // PUT: api/salas/{salaId}/rotar-codigo
        [HttpPut("{salaId}/rotar-codigo")]
        public async Task<ActionResult<RotarCodigoResponse>> RotarCodigo(int salaId, [FromBody] string nuevoCodigo)
        {
            var resultado = await _svSalas.RotarCodigoAsync(salaId, nuevoCodigo);
            return Ok(resultado);
        }

        // GET: api/salas/{salaId}
        [HttpGet("{salaId}")]
        public async Task<ActionResult<SalaDto>> ObtenerSala(int salaId)
        {
            var sala = await _svSalas.ObtenerPorIdAsync(salaId);
            return Ok(sala);
        }

        // GET: api/salas/{salaId}/marcador
        [HttpGet("{salaId}/marcador")]
        public async Task<ActionResult<List<LobbyScoreDto>>> ObtenerMarcador(int salaId)
        {
            var marcador = await _svSalas.MarcadorAsync(salaId);
            return Ok(marcador);
        }

        // GET: api/salas/codigo/{codigo}
        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<SalaDto>> ObtenerSalaPorCodigo(string codigo)
        {
            var codigoUpper = codigo.Trim().ToUpperInvariant();

            var sala = await _db.Salas
                .Include(s => s.Jugadores)
                    .ThenInclude(sj => sj.Jugador)
                .Include(s => s.Partidas)
                .SingleOrDefaultAsync(s => s.CodigoIngreso == codigoUpper);

            if (sala == null)
                throw new NotFoundException($"Sala con código {codigo} no existe.");

            return Ok(sala.ADto());
        }
    }
}