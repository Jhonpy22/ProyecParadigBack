using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Turnos
{
    public class UnaReveladaState : IEstadoTurno
    {
        public Task VoltearAsync(Partida p, int jugadorId, int indice)
        {
            if (FinalizacionDePartida.DebeExpirar(p, DateTime.UtcNow))
            {
                FinalizacionDePartida.FinalizarYVolverAlLobby(p);
                return Task.CompletedTask;
            }

            ValidacionesDeTurno.AsegurarTurnoValido(p, jugadorId);

            if (p.IndicePrimerVolteo is null)
                throw new InvalidOperationException("Estado inconsistente: no hay primer volteo.");

            if (p.IndicePrimerVolteo.Value == indice)
                throw new InvalidOperationException("No puedes seleccionar la misma carta.");

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
                gp.Puntaje += 1;

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
                
                p.IndicePrimerVolteo = null;
                p.JugadorActualId = NextPlayerId(p);
                p.NumeroTurno++;
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

