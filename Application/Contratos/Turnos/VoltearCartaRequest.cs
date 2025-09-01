

namespace Application.Contratos.Turnos
{
    public sealed record VoltearCartaRequest(
    int PartidaId,
    int JugadorId,
    int Indice
    );
}
