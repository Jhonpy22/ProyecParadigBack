using Domain.Enums;

namespace Application.Contratos.Salas
{
    public sealed record SalaDto(
    int SalaId,
    string CodigoIngreso,
    EstadoSala Estado,
    int MaxJugadores,
    int AnfitrionId,
    int? PartidaActualId,
    List<SalaJugadorDto> Jugadores
    );
}
