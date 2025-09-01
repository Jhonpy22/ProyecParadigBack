

namespace Application.Contratos.Partidas
{
    public sealed record PartidaJugadorDto(
     int JugadorId,
     string Nombre,
     int Puntaje,
     int OrdenTurno
    );
}
