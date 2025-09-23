using Application.Common.Exceptions;
using Application.Contratos.Partidas;
using Application.Interfaces;
using Application.Mapeos;
using Domain.Entities;
using Domain.Enums;
using Domain.States.Salas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace ServicesApp
{
    public sealed class SvPartidas : ISvPartidas
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SvPartidas> _logger;

        public SvPartidas(AppDbContext db, ILogger<SvPartidas> logger)
        {
            _db = db;
            _logger = logger;
        }

    
        public async Task<PartidaDto> IniciarAsync(IniciarPartidaRequest req)
        {

            _logger.LogInformation("Iniciando partida en SalaId={SalaId}, Dificultad={Dificultad}", req.SalaId, req.Dificultad);

            var sala = await _db.Salas
                .Include(s => s.Jugadores)
                    .ThenInclude(sj => sj.Jugador)
                .SingleOrDefaultAsync(s => s.SalaId == req.SalaId)
            ?? throw new NotFoundException("Sala no existe.");

            if (sala.Estado == EstadoSala.Finalizada)
            {
                _logger.LogWarning("Intento de iniciar partida en sala finalizada SalaId={SalaId}", sala.SalaId);
                throw new BusinessException("No se puede iniciar una partida en una sala finalizada.");
            }
            if (sala.Estado == EstadoSala.EnJuego)
            {
                _logger.LogWarning("Intento de iniciar nueva partida cuando ya hay una en curso en SalaId={SalaId}", sala.SalaId);
                throw new BusinessException("No se puede iniciar una nueva partida mientras hay una en curso.");
            }

            int dFilas, dColumnas, dDuracion, dPuntos;
            switch (req.Dificultad)
            {
                case Dificultad.Facil: dFilas = 2; dColumnas = 4; dDuracion = 90; dPuntos = 1; break;
                case Dificultad.Medio: dFilas = 4; dColumnas = 4; dDuracion = 120; dPuntos = 2; break;
                case Dificultad.Dificil: dFilas = 4; dColumnas = 6; dDuracion = 150; dPuntos = 3; break;
                default:
                    _logger.LogWarning("Dificultad {Dificultad} no soportada en SalaId={SalaId}", req.Dificultad, req.SalaId);
                    throw new BusinessException("Dificultad no soportada.");
            }

        
            await FabricaEstadoSala.From(sala).IniciarAsync(sala, dFilas, dColumnas);

            var partida = req.Construir(sala, dFilas, dColumnas, dDuracion);

       
            partida.EstablecerPuntosPorPareja(dPuntos);

       

        
            var jugadoresOrdenados = sala.Jugadores
                .OrderBy(j => j.OrdenTurno ?? int.MaxValue)
                .ToList();

            if (jugadoresOrdenados.Count < 2)
            {
                _logger.LogWarning("Intento de iniciar partida sin jugadores suficientes en SalaId={SalaId}", req.SalaId);
                throw new BusinessException("Se requieren al menos 2 jugadores para iniciar.");
            }

            int orden = 1;
            foreach (var sj in jugadoresOrdenados)
            {
                partida.PartidaJugadores.Add(new PartidaJugador
                {
                    Partida = partida,
                    JugadorId = sj.JugadorId,
                    Jugador = sj.Jugador,
                    OrdenTurno = orden++,
                    Puntaje = 0
                });
            }

        
            var totalCartas = dFilas * dColumnas;
            var cartas = GenerarTablero(totalCartas); 
            foreach (var c in cartas)
                partida.Tablero.Add(c);

        
            sala.Partidas.Add(partida);
            sala.PartidaActual = partida;

        
            await _db.SaveChangesAsync();


            _logger.LogInformation("Partida {PartidaId} iniciada en SalaId={SalaId} con {Jugadores} jugadores, Tablero={Filas}x{Columnas}, Tiempo={Tiempo}s, PuntosPorPareja={Puntos}",
              partida.PartidaId, sala.SalaId, jugadoresOrdenados.Count, dFilas, dColumnas, dDuracion, dPuntos);

            return partida.ADto();
        }

   
        public async Task<PartidaDto> ObtenerAsync(int partidaId)
        {
            _logger.LogInformation("Obteniendo partida PartidaId={PartidaId}", partidaId);

            var p = await _db.Partidas
                .Include(x => x.Sala)
                .Include(x => x.PartidaJugadores)
                    .ThenInclude(pj => pj.Jugador)
                .Include(x => x.Tablero)
                .Include(x => x.Movimientos)
                .SingleOrDefaultAsync(x => x.PartidaId == partidaId);

            if (p == null)
            {
                _logger.LogWarning("PartidaId={PartidaId} no existe", partidaId);
                throw new NotFoundException($"Partida con id {partidaId} no existe.");
            }
            return p.ADto();
        }


        private static List<CartaTablero> GenerarTablero(int totalCartas)
        {
        
            var claves = new int[totalCartas];
            int pareja = 0;
            for (int i = 0; i < totalCartas; i += 2)
            {
                claves[i] = pareja;
                claves[i + 1] = pareja;
                pareja++;
            }

        
            ShuffleInPlace(claves);

       
            var cartas = new List<CartaTablero>(totalCartas);
            for (int i = 0; i < totalCartas; i++)
            {
                cartas.Add(new CartaTablero
                {
                    Indice = i,
                    ClavePareja = claves[i],
                    EstaEmparejada = false
                });
            }
            return cartas;
        }

    
        private static void ShuffleInPlace(int[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = GetRandomInt(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

   
        private static int GetRandomInt(int minInclusive, int maxExclusive)
        {
            if (minInclusive >= maxExclusive) return minInclusive;
            var range = (uint)(maxExclusive - minInclusive);
            var scale = GetRandomUInt() / (double)uint.MaxValue;
            return (int)(minInclusive + (uint)(scale * range));
        }

        private static uint GetRandomUInt()
        {
            Span<byte> bytes = stackalloc byte[4];
            RandomNumberGenerator.Fill(bytes);
            return BitConverter.ToUInt32(bytes);
        }
    }
}
