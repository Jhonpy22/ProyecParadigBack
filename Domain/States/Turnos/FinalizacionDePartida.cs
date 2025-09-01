using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Turnos
{
    internal static class FinalizacionDePartida
    {
        public static bool DebeExpirar(Partida p, DateTime ahoraUtc)
            => p.DuracionSegundos > 0 && p.Expirada(ahoraUtc);

        public static void Finalizar(Partida p)
        {
            if (p.Estado == EstadoPartida.Finalizada) return;

            p.Estado = EstadoPartida.Finalizada;
            p.FinalizadaUtc = DateTime.UtcNow;

            var max = p.PartidaJugadores.Max(x => x.Puntaje);
            var candidatos = p.PartidaJugadores.Where(x => x.Puntaje == max).ToList();

            PartidaJugador ganadorPj;
            if (candidatos.Count == 1)
            {
                ganadorPj = candidatos[0];
            }
            else
            {
                var ultimos = p.Movimientos
                               .Where(m => m.FuePareja)
                               .GroupBy(m => m.JugadorId)
                               .ToDictionary(g => g.Key, g => g.Max(m => m.CreadoUtc));

                ganadorPj = candidatos
                    .OrderBy(c => ultimos.TryGetValue(c.JugadorId, out var t) ? t : DateTime.MaxValue)
                    .ThenBy(c => c.OrdenTurno)
                    .First();
            }

            p.GanadorId = ganadorPj.JugadorId;
            p.PuntajeGanador = ganadorPj.Puntaje;
        }

        public static void FinalizarYVolverAlLobby(Partida p)
        {
            Finalizar(p);
            p.Sala.Estado = EstadoSala.Lobby;
            p.Sala.PartidaActual = null;
            p.Sala.PartidaActualId = null;
        }
    }
}
