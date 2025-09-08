
namespace Application.Contratos.Turnos
{
    public sealed record MovimientoDto(
    int PartidaId,
    int JugadorId,
    int IndicePrimero,
    int IndiceSegundo,
    bool FuePareja,
    DateTime CreadoUtc
    );
}
