

namespace Application.Contratos.Turnos
{
    public sealed record VoltearCartaRequest(
    int PartidaId,
    string NombreJugador,
    int Indice
    );
}
