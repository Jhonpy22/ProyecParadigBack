using Domain.Enums;


namespace Application.Contratos.Partidas
{

    public sealed record IniciarPartidaRequest
    {
        public int SalaId { get; init; }
        public Dificultad Dificultad { get; init; }
    }

}
