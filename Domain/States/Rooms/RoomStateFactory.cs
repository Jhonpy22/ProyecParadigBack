using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Rooms
{
    public static class RoomStateFactory
    {
        public static IRoomState From(Room room) =>
            room.Status switch
            {
                RoomStatus.Lobby => new LobbyState(),
                RoomStatus.InGame => new InGameState(),
                RoomStatus.Finished => new FinishedState(),
                _ => throw new InvalidOperationException("RoomStatus inválido.")
            };
    }
}
