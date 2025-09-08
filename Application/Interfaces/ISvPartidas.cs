using Application.Contratos.Partidas;

namespace Application.Interfaces
{
    public interface ISvPartidas
    {
        Task<PartidaDto> IniciarAsync(IniciarPartidaRequest req);
        Task<PartidaDto> ObtenerAsync(int partidaId);
    }
}
