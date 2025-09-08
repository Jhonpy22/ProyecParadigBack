
namespace Application.Contratos.Salas
{
    public sealed record CrearSalaRequest(
     string NombreJugador,
    int MaxJugadores,
    string? CodigoIngreso
    );
}
