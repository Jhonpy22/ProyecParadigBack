using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Turnos
{
    public class UnaReveladaState : IEstadoTurno
    {
        public Task VoltearAsync(Partida p, int jugadorId, int indice)
        {
            var tiempoTranscurrido = (DateTime.UtcNow - p.IniciadaUtc).TotalSeconds;
            Console.WriteLine($"TIEMPO CHECK - Transcurrido: {tiempoTranscurrido:F1}s de {p.DuracionSegundos}s, Expirada: {p.Expirada(DateTime.UtcNow)}");

            if (FinalizacionDePartida.DebeExpirar(p, DateTime.UtcNow))
            {
                Console.WriteLine("PARTIDA EXPIRADA - Finalizando por tiempo agotado");
                FinalizacionDePartida.FinalizarYVolverAlLobby(p);
                return Task.CompletedTask;
            }

            ValidacionesDeTurno.AsegurarTurnoValido(p, jugadorId);

            if (p.IndicePrimerVolteo is null)
                throw new InvalidOperationException("Estado inconsistente: no hay primer volteo.");

            /*if (p.IndicePrimerVolteo.Value == indice)
                throw new InvalidOperationException("No puedes seleccionar la misma carta.");*/

            var primera = GetPlayableCard(p, p.IndicePrimerVolteo.Value);
            var segunda = GetPlayableCard(p, indice);

            var esPareja = primera.ClavePareja == segunda.ClavePareja;

            p.Movimientos.Add(new Movimiento
            {
                PartidaId = p.PartidaId,
                JugadorId = jugadorId,
                IndicePrimero = primera.Indice,
                IndiceSegundo = segunda.Indice,
                FuePareja = esPareja,
                CreadoUtc = DateTime.UtcNow
            });

            if (esPareja)
            {
                primera.EstaEmparejada = true;
                segunda.EstaEmparejada = true;

                var gp = p.PartidaJugadores.Single(x => x.JugadorId == jugadorId);
                gp.Puntaje += p.PuntosPorPareja;

                if (p.Tablero.All(c => c.EstaEmparejada))
                {
                    FinalizacionDePartida.FinalizarYVolverAlLobby(p);
                    return Task.CompletedTask;

                }
                p.IndicePrimerVolteo = null;
                return Task.CompletedTask;

            }
            else
            {
                Console.WriteLine($"ANTES DEL CAMBIO - JugadorActual: {p.JugadorActualId}, Turno: {p.NumeroTurno}");
                if (p.JugadorActualId.HasValue)
                {
                    p.JugadorActualId = NextPlayerId(p);
                }

                p.NumeroTurno++;

                p.IndicePrimerVolteo = null;

                Console.WriteLine($"DESPUÉS DEL CAMBIO - JugadorActual: {p.JugadorActualId}, Turno: {p.NumeroTurno}");
                return Task.CompletedTask;
            }
        }
        private static CartaTablero GetPlayableCard(Partida p, int indice)
        {
            var carta = p.Tablero.SingleOrDefault(c => c.Indice == indice)
                        ?? throw new InvalidOperationException("Índice de carta inválido.");
            if (carta.EstaEmparejada)
                throw new InvalidOperationException("La carta ya está tomada.");
            return carta;
        }

        private static int NextPlayerId(Partida p)
        {
            var ordered = p.PartidaJugadores
                           .OrderBy(x => x.OrdenTurno)
                           .Select(x => x.JugadorId)
                           .ToList();
            var pos = ordered.IndexOf(p.JugadorActualId!.Value);
            return ordered[(pos + 1) % ordered.Count];
        }
    }
}

