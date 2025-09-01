using Domain.Entities;


namespace Domain.States.Salas
{
    public interface IEstadoSala
    {
        Task UnirseAsync(Sala sala, Jugador jugador);
        Task IniciarAsync(Sala sala, int filas, int columnas);
        Task FinalizarAsync(Sala sala);
    }
}
