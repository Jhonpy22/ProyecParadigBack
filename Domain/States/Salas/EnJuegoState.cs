using Domain.Entities;
using Domain.Enums;

namespace Domain.States.Salas
{
    public class InGameState : IEstadoSala
    {
        public Task UnirseAsync(Sala sala, Jugador jugador)
            => throw new InvalidOperationException("No se puede unir mientras la partida esté en curso.");

        public Task IniciarAsync(Sala sala, int filas, int columnas)
            => throw new InvalidOperationException("La sala ya está en partida.");

        public Task FinalizarAsync(Sala sala)
        {
            sala.Estado = EstadoSala.Finalizada;
            return Task.CompletedTask;
        }
    }
}
