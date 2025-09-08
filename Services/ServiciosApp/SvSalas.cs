using Application.Common.Exceptions;
using Application.Contratos.Lobby;
using Application.Contratos.Salas;
using Application.Interfaces;
using Application.Mapeos;
using Domain.Entities;
using Domain.States.Salas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace ServicesApp
{
    
    public sealed class SvSalas : ISvSalas
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SvSalas> _logger;

        public SvSalas(AppDbContext db, ILogger<SvSalas> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<SalaDto> CrearAsync(CrearSalaRequest req)
        {
            _logger.LogInformation("Creando sala: CreadorId={CreadorId}, MaxJugadores={MaxJugadores}", req.NombreJugador, req.MaxJugadores);

                
            var anfitrion = await _db.Jugadores
                .SingleOrDefaultAsync(j => j.Nombre == req.NombreJugador);

            if (anfitrion == null)
            {
                anfitrion = new Jugador
                {
                    Nombre = req.NombreJugador,
                    
                };
                _db.Jugadores.Add(anfitrion);
                await _db.SaveChangesAsync();
            }

            var sala = req.Construir(anfitrion);


            if (string.IsNullOrWhiteSpace(sala.CodigoIngreso))
            {
                var up = sala.CodigoIngreso.Trim().ToUpperInvariant();
                var existe = await _db.Salas.AnyAsync(s => s.CodigoIngreso == up);
                if (existe)
                {
                    _logger.LogWarning("Conflicto de código al crear sala: {CodigoIngreso}", up);
                    throw new ConflictException($"El código {up} ya está en uso.");
                }
                sala.EstablecerCodigoIngreso(up);
            }
            else
            {
                
                var code = await GenerarCodigoUnicoAsync(8, 20);
                sala.EstablecerCodigoIngreso(code);
            }

            sala.Jugadores.Add(new SalaJugador
            {
                Sala = sala,
                Jugador = anfitrion,
                OrdenTurno = 1
            });

            _db.Salas.Add(sala);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Sala creada: SalaId={SalaId}, Codigo={Codigo}, AnfitrionId={AnfitrionId}", sala.SalaId, sala.CodigoIngreso, sala.AnfitrionId);

            return sala.ADto();
        }

        public async Task<SalaDto> UnirseAsync(UnirseSalaRequest req)
        {

            var codigo = req.CodigoIngreso.Trim().ToUpperInvariant();

            _logger.LogInformation("Unirse a sala: Codigo={Codigo}, JugadorId={JugadorId}", codigo, req.NombreJugador);

            var sala = await _db.Salas.Include(s => s.Jugadores).ThenInclude(sj => sj.Jugador)
                .SingleOrDefaultAsync(s => s.CodigoIngreso == codigo);

            if (sala == null)
                throw new NotFoundException("Sala no encontrada por código.");

            var jugador = await _db.Jugadores

                .SingleOrDefaultAsync(j => j.Nombre == req.NombreJugador);
            if (jugador == null)
            {
                jugador = new Jugador { Nombre = req.NombreJugador };
                _db.Jugadores.Add(jugador);
                await _db.SaveChangesAsync(); 
            }

            await FabricaEstadoSala.From(sala).UnirseAsync(sala, jugador);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Jugador {JugadorId} se unió a SalaId={SalaId}", req.NombreJugador, sala.SalaId);
            return sala.ADto();
        }

        public async Task<RotarCodigoResponse> RotarCodigoAsync(int salaId, string nuevoCodigo)
        {
            var up = (nuevoCodigo ?? string.Empty).Trim().ToUpperInvariant();

            _logger.LogInformation("Rotar código: SalaId={SalaId}, NuevoCodigo={Codigo}", salaId, up);

            var sala = await _db.Salas.SingleOrDefaultAsync(s => s.SalaId == salaId);


            if (sala == null)
                throw new NotFoundException("Sala no existe.");

            sala.EstablecerCodigoIngreso(up);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Código rotado: SalaId={SalaId}, Codigo={Codigo}", sala.SalaId, sala.CodigoIngreso);
            return sala.ARespuestaRotacion();
        }

        public async Task<SalaDto> ObtenerPorIdAsync(int salaId)
        {
            _logger.LogInformation("Obtener sala: SalaId={SalaId}", salaId);

            var sala = await _db.Salas
                .Include(s => s.Jugadores)
                    .ThenInclude(sj => sj.Jugador)
                .Include(s => s.Partidas)
                .SingleOrDefaultAsync(s => s.SalaId == salaId);

            if (sala == null)
                throw new NotFoundException("Sala no existe.");

            return sala.ADto();
        }

        public async Task<List<LobbyScoreDto>> MarcadorAsync(int salaId)
        {
            _logger.LogInformation("Marcador acumulado: SalaId={SalaId}", salaId);

            var sala = await _db.Salas
                .Include(s => s.Partidas)
                    .ThenInclude(p => p.PartidaJugadores)
                        .ThenInclude(pj => pj.Jugador)
                .SingleOrDefaultAsync(s => s.SalaId == salaId);

            if (sala == null)
                throw new NotFoundException("Sala no existe.");

            return sala.MarcadorAcumulado();
        }

       

        private async Task<string> GenerarCodigoUnicoAsync(int length, int maxIntentos)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (int intento = 0; intento < maxIntentos; intento++)
            {
                var code = RandomString(alphabet, length);
                var existe = await _db.Salas.AnyAsync(s => s.CodigoIngreso == code);
                if (!existe) return code;
            }

            _logger.LogWarning("No fue posible generar un código único tras {Intentos} intentos.", maxIntentos);

            
            var g = Guid.NewGuid().ToString("N").ToUpperInvariant();
            return g.Substring(0, length);
        }

        private static string RandomString(string alphabet, int length)
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(alphabet[bytes[i] % alphabet.Length]);
            }
            return sb.ToString();
        }
    }
}
