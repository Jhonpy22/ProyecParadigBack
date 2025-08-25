using Domain.Entities;


namespace Domain.States.Rooms
{
    public class FinishedState : IRoomState
    {
        public Task JoinAsync(Room room, Player player)
            => throw new InvalidOperationException("La sala ya finalizó.");

        public Task StartAsync(Room room, int rows, int cols)
            => throw new InvalidOperationException("La sala ya finalizó; no se puede iniciar otra partida.");

        public Task FinishAsync(Room room) => Task.CompletedTask; // idempotente
    }
}
