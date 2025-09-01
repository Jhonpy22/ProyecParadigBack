using Domain.Entities;


namespace Domain.States.Turnos
{
    public interface IEstadoTurno
    {
        Task VoltearAsync(Partida partida, int jugadorId, int indice);
    }

}
