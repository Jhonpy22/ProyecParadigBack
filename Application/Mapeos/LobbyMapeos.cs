using Application.Contratos.Lobby;
using Domain.Entities;


namespace Application.Mapeos
{
    public static class LobbyMapeos
    {
        
        public static List<LobbyScoreDto> MarcadorAcumulado(this Sala sala)
            => sala.Partidas
                .SelectMany(p => p.PartidaJugadores)
                .GroupBy(pj => pj.JugadorId)
                .Select(g => new LobbyScoreDto(
                    JugadorId: g.Key,
                    Nombre: g.Select(x => x.Jugador.Nombre).FirstOrDefault() ?? string.Empty,
                    PuntosAcumulados: g.Sum(x => x.Puntaje)
                ))
                .OrderByDescending(x => x.PuntosAcumulados)
                .ToList();
    }
}
