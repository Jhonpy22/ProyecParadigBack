using Application.Common.Exceptions;
using Application.Contratos.Partidas;
using Application.Contratos.Turnos;
using Application.Interfaces;
using Application.Mapeos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ServicesApp
{
    
    public sealed class SvTurnos : ISvTurnos
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SvTurnos> _logger;

        public SvTurnos(AppDbContext db, ILogger<SvTurnos> logger)
        {
            _db = db;
            _logger = logger;
        }

      
        public async Task<PartidaDto> VoltearAsync(VoltearCartaRequest req)
        {
            _logger.LogInformation("Voltear carta: PartidaId={PartidaId}, JugadorId={JugadorId}, Indice={Indice}",
               req.PartidaId, req.NombreJugador, req.Indice);

            var p = await _db.Partidas
                .Include(x => x.Sala)
                .Include(x => x.PartidaJugadores)
                    .ThenInclude(pj => pj.Jugador)
                .Include(x => x.Tablero)
                .Include(x => x.Movimientos)
                .SingleOrDefaultAsync(x => x.PartidaId == req.PartidaId);

            if (p == null)
                throw new NotFoundException("Partida no existe.");

           
            await req.AplicarAsync(p);

            
            await _db.SaveChangesAsync();

            var mov = p.Movimientos.OrderByDescending(m => m.CreadoUtc).FirstOrDefault();
            if (mov is not null)
            {
                _logger.LogInformation(
                    "Movimiento registrado: PartidaId={PartidaId}, JugadorId={JugadorId}, Indices=({Primero},{Segundo}), FuePareja={FuePareja}, Turno={Turno}, EstadoPartida={Estado}",
                    mov.PartidaId, mov.JugadorId, mov.IndicePrimero, mov.IndiceSegundo, mov.FuePareja, p.NumeroTurno, p.Estado);
            }
            else
            {
                
                _logger.LogWarning("No se encontró movimiento tras aplicar VoltearCarta. PartidaId={PartidaId}", req.PartidaId);
            }


            return p.ADto();
        }
    }
}
