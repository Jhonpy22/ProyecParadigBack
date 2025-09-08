

namespace Application.Contratos.Lobby
{
    public sealed record LobbyScoreDto(
     int JugadorId,
     string Nombre,
     int PuntosAcumulados
    );
}
