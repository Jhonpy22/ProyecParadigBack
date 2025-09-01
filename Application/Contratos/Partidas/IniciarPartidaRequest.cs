using Domain.Enums;


namespace Application.Contratos.Partidas
{

    public sealed record IniciarPartidaRequest(
        int SalaId,
        Dificultad Dificultad
    );
}
