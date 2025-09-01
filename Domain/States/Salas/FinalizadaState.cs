using Domain.Entities;


namespace Domain.States.Salas
{
    public class FinishedState : IEstadoSala
    {
        public Task UnirseAsync(Sala sala, Jugador jugador)
            => throw new InvalidOperationException("La sala ya finalizó.");

        public Task IniciarAsync(Sala sala, int filas, int columnas)
            => throw new InvalidOperationException("La sala ya finalizó; no se puede iniciar otra partida.");

        public Task FinalizarAsync(Sala sala) => Task.CompletedTask;
    }
}
