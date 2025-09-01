
namespace Application.Contratos.Salas
{
    public sealed record CrearSalaRequest(
    int CreadorId,
    int MaxJugadores,
    string? CodigoIngreso
    );
}
