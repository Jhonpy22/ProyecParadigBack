using Application.Contratos.Partidas;
using Application.Contratos.Turnos;

namespace Application.Interfaces
{
    public interface ISvTurnos
    {
        Task<PartidaDto> VoltearAsync(VoltearCartaRequest req);
    }
}
