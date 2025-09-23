namespace Application.Contratos.Partidas
{
    public sealed record CartaTableroDto(
        int Indice,
        int ClavePareja,
        bool EstaEmparejada
    );
}