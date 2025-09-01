using Application.Contratos.Salas;
using Domain.Entities;


namespace Application.Mapeos
{
    public static class SalaMapeos
    {
        
        public static SalaDto ADto(this Sala s)
            => new(
                SalaId: s.SalaId,
                CodigoIngreso: s.CodigoIngreso,
                Estado: s.Estado,
                MaxJugadores: s.MaxJugadores,
                AnfitrionId: s.AnfitrionId,
                PartidaActualId: s.PartidaActualId,
                Jugadores: s.Jugadores
                    .OrderBy(j => j.OrdenTurno ?? int.MaxValue)
                    .Select(j => new SalaJugadorDto(
                        JugadorId: j.JugadorId,
                        Nombre: j.Jugador.Nombre,
                        OrdenTurno: j.OrdenTurno
                    ))
                    .ToList()
            );
    }
}
