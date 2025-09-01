using Domain.Entities;
using Domain.Enums;

namespace Domain.States.Salas
{
    public static class FabricaEstadoSala
    {
        public static IEstadoSala From(Sala sala) =>
            sala.Estado switch
            {
                EstadoSala.Lobby => new LobbyState(),
                EstadoSala.EnJuego => new InGameState(),
                EstadoSala.Finalizada => new FinishedState(),
                _ => throw new InvalidOperationException("Estado de la sala inválido.")
            };
    }
}
