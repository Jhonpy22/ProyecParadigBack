using Domain.Entities;
using Domain.Enums;



namespace Domain.States.Turnos
{
    public class DesocupadoState : IEstadoTurno
    {
        public Task VoltearAsync(Partida j, int jugadorId, int indice)
        {

            var tiempoTranscurrido = (DateTime.UtcNow - j.IniciadaUtc).TotalSeconds;
            Console.WriteLine($"TIEMPO CHECK - Transcurrido: {tiempoTranscurrido:F1}s de {j.DuracionSegundos}s, Expirada: {j.Expirada(DateTime.UtcNow)}");

            if (FinalizacionDePartida.DebeExpirar(j, DateTime.UtcNow))
            {
                Console.WriteLine("PARTIDA EXPIRADA - Finalizando por tiempo agotado");
                FinalizacionDePartida.FinalizarYVolverAlLobby(j);
                return Task.CompletedTask;
            }


            ValidacionesDeTurno.AsegurarTurnoValido(j, jugadorId);

            var carta = j.Tablero.SingleOrDefault(c => c.Indice == indice)
                        ?? throw new InvalidOperationException("Índice de carta inválido.");

            if (carta.EstaEmparejada)
                throw new InvalidOperationException("La carta ya está tomada.");

            j.IndicePrimerVolteo = carta.Indice;
            return Task.CompletedTask;

        }
    }
}
