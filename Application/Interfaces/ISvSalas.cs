using Application.Contratos.Lobby;
using Application.Contratos.Salas;

namespace Application.Interfaces
{
    public interface ISvSalas
    {

        Task<SalaDto> CrearAsync(CrearSalaRequest req);

        Task<SalaDto> UnirseAsync(UnirseSalaRequest req);

        Task<RotarCodigoResponse> RotarCodigoAsync(int salaId, string nuevoCodigo);

        Task<SalaDto> ObtenerPorIdAsync(int salaId);

        Task<List<LobbyScoreDto>> MarcadorAsync(int salaId);
    }
}
