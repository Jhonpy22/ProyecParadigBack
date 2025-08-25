using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Rooms
{
    public class LobbyState : IRoomState
    {
        public Task JoinAsync(Room room, Player player)
        {
            if (room.Status != RoomStatus.Lobby)
                throw new InvalidOperationException("No se puede unir: la sala no está en Lobby.");
            // La inserción real (RoomPlayer) la hace el servicio.
            return Task.CompletedTask;
        }

        public Task StartAsync(Room room, int rows, int cols)
        {
            if (room.Status != RoomStatus.Lobby)
                throw new InvalidOperationException("No se puede iniciar: la sala no está en Lobby.");

            if (rows <= 0 || cols <= 0 || (rows * cols) % 2 != 0)
                throw new InvalidOperationException("Dimensiones inválidas (rows*cols debe ser par).");

            if (room.Players is null || room.Players.Count < 2)
                throw new InvalidOperationException("Se requieren al menos 2 jugadores.");

            // La creación del Game la hace el servicio; aquí solo cambiamos estado.
            room.Status = RoomStatus.InGame;
            return Task.CompletedTask;
        }

        public Task FinishAsync(Room room)
            => throw new InvalidOperationException("No se puede finalizar una sala que no ha iniciado.");
    }
}
