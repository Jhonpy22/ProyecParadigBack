using Application.Contratos.Partidas;

namespace Application.Interfaces
{
    public interface INotificadorJuego
    {
        Task EnviarPartidaFinalizadaAsync(PartidaDto partida, string motivo);
    }
}
