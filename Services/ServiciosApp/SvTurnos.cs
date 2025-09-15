using Application.Common.Exceptions;
using Application.Contratos.Partidas;
using Application.Contratos.Turnos;
using Application.Interfaces;
using Application.Mapeos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace ServicesApp
{
    
    public sealed class SvTurnos : ISvTurnos
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SvTurnos> _logger;
        private readonly INotificadorJuego _notificador;


        public SvTurnos(AppDbContext db, ILogger<SvTurnos> logger, INotificadorJuego notificador)
        {
            _db = db;
            _logger = logger;
            _notificador = notificador;
        }

      
        public async Task<PartidaDto> VoltearAsync(VoltearCartaRequest req)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try {

                _logger.LogInformation("Voltear carta: PartidaId={PartidaId}, JugadorId={JugadorId}, Indice={Indice}",
                   req.PartidaId, req.NombreJugador, req.Indice);

                var p = await CargarPartidaCompletaAsync(req.PartidaId);

                
                await req.AplicarAsync(p);

               
                var resultadoFinalizacion = await ProcesarFinalizacionAsync(p);

                
                await _db.SaveChangesAsync();
                _db.Entry(p).State = EntityState.Detached;
                await transaction.CommitAsync();

                _logger.LogInformation("Volteo completado - EstadoPartida:{Estado}, SalaEstado:{SalaEstado}",
                    p.Estado, p.Sala.Estado);

                
                _ = Task.Run(async () => await EnviarNotificacionesAsync(p, resultadoFinalizacion));

                return p.ADto();

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error en volteo - Partida:{PartidaId}, Jugador:{Jugador}",
                req.PartidaId, req.NombreJugador);

                await transaction.RollbackAsync();
                throw;
            }

        }


        private async Task<Partida> CargarPartidaCompletaAsync(int partidaId)
        {
            var p = await _db.Partidas
                .Include(x => x.Sala)
                .Include(x => x.PartidaJugadores).ThenInclude(pj => pj.Jugador)
                .Include(x => x.Tablero)
                .Include(x => x.Movimientos)
                .SingleOrDefaultAsync(x => x.PartidaId == partidaId);

            if (p == null)
                throw new NotFoundException("Partida no existe.");

            return p;
        }

        private async Task<ResultadoFinalizacion> ProcesarFinalizacionAsync(Partida p)
        {
            var resultado = new ResultadoFinalizacion { PartidaFinalizada = false };

            if (p.Estado == EstadoPartida.Finalizada)
            {
                p.Sala.Estado = EstadoSala.Finalizada;
                
                p.Sala.PartidaActual = null;
                p.Sala.PartidaActualId = null;

                if (!p.FinalizadaUtc.HasValue)
                {
                    p.FinalizadaUtc = DateTime.UtcNow;
                }

                resultado.PartidaFinalizada = true;
                resultado.Motivo = DeterminarMotivoFinalizacion(p);
                resultado.SalaCodigo = p.Sala.CodigoIngreso;
                resultado.PartidaDto = p.ADto();
            }

            return resultado;
        }


        private async Task EnviarNotificacionesAsync(Partida p, ResultadoFinalizacion resultado)
        {
            try
            {
                if (resultado.PartidaFinalizada)
                {
                    await _notificador.EnviarPartidaFinalizadaAsync(
                        resultado.PartidaDto,
                        resultado.Motivo
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error enviando notificaciones - Partida:{PartidaId}", p.PartidaId);
                
            }
        }

        private string DeterminarMotivoFinalizacion(Partida p)
        {
            if (p.Expirada(DateTime.UtcNow))
                return "Tiempo agotado";

            if (p.Tablero.All(c => c.EstaEmparejada))
                return "Juego completado";

            return "Partida finalizada";
        }
    }

    
    public class ResultadoFinalizacion
    {
        public bool PartidaFinalizada { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string SalaCodigo { get; set; } = string.Empty;
        public PartidaDto PartidaDto { get; set; } = null!;
    }


      



   
}
