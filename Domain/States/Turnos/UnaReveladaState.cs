using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Turnos
{
    public class UnaReveladaState : IEstadoTurno
    {
        public Task VoltearAsync(Partida p, int jugadorId, int indice)
        {
            
                if (p.Expirada(DateTime.UtcNow))
                    throw new InvalidOperationException("La partida expiró.");

                EnsurePlayerTurn(p, jugadorId);

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

                   
                    p.IndicePrimerVolteo = null;      
                }
                else
                {
                    
                    p.IndicePrimerVolteo = null;
                    p.JugadorActualId = NextPlayerId(p);
                    p.NumeroTurno++;
                }

                return Task.CompletedTask;
        }
        
        private static void EnsurePlayerTurn(Partida p, int jugadorId)
        {
            if (p.Estado != EstadoPartida.EnProgreso)
                throw new InvalidOperationException("La partida no está en progreso.");
            if (p.JugadorActualId != jugadorId)
                throw new InvalidOperationException("No es tu turno.");
        }

        private static CartaTablero GetPlayableCard(Partida p, int indice)
        {
            var carta = p.Tablero.SingleOrDefault(c => c.Indice == indice) ?? throw new InvalidOperationException("Índice de carta inválido.");
            if (carta.EstaEmparejada) throw new InvalidOperationException("La carta ya está tomada.");
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

