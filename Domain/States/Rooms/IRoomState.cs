using Domain.Entities;


namespace Domain.States.Rooms
{
    public interface IRoomState
    {
        Task JoinAsync(Room room, Player player);
        Task StartAsync(Room room, int rows, int cols);
        Task FinishAsync(Room room);
    }
}
