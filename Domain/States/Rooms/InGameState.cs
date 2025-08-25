using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Rooms
{
    public class InGameState : IRoomState
    {
        public Task JoinAsync(Room room, Player player)
            => throw new InvalidOperationException("No se puede unir mientras el juego está en curso.");

        public Task StartAsync(Room room, int rows, int cols)
            => throw new InvalidOperationException("La sala ya está en juego.");

        public Task FinishAsync(Room room)
        {
            room.Status = RoomStatus.Finished;
            return Task.CompletedTask;
        }
    }
}
