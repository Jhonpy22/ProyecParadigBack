using Domain.Entities;
using Domain.Enums;



namespace Domain.States.Turnos
{
    public class DesocupadoState : IEstadoTurno
    {
        public Task VoltearAsync(Partida j, int jugadorId, int indice)
        {
            if (j.Expirada(DateTime.UtcNow))
                throw new InvalidOperationException("La partida expiró.");

            if (j.Estado != EstadoPartida.EnProgreso)
            {
                throw new InvalidOperationException("La partida no está en progreso.");
            }
            if (j.JugadorActualId != jugadorId)
            {
                throw new InvalidOperationException("No es tu turno.");
            }
            var carta = j.Tablero.SingleOrDefault(c => c.Indice == indice) ?? throw new InvalidOperationException("Índice inválido.");

            if (carta.EstaEmparejada) throw new InvalidOperationException("Carta ya tomada.");
            j.IndicePrimerVolteo = carta.Indice;
            return Task.CompletedTask;

        }
    }
}
