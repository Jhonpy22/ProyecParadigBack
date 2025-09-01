

namespace Application.Contratos.Salas
{
    public sealed record SalaJugadorDto
    (
    int JugadorId,
    string Nombre,
    int? OrdenTurno
    );
}
