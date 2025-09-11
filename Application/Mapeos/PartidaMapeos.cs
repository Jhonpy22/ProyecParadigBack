using Application.Contratos.Partidas;
using Application.Contratos.Turnos;
using Domain.Entities;

namespace Application.Mapeos
{
    public static class PartidaMapeos
    {
        
        public static PartidaDto ADto(this Partida p)
            => new(
                PartidaId: p.PartidaId,
                SalaId: p.SalaId,
                SalaCodigo: p.Sala.CodigoIngreso,
                Estado: p.Estado,
                Dificultad: p.Dificultad,
                Filas: p.Filas,
                Columnas: p.Columnas,
                DuracionSegundos: p.DuracionSegundos,
                IniciadaUtc: p.IniciadaUtc,
                JugadorActualId: p.JugadorActualId!.Value,
                NumeroTurno: p.NumeroTurno,
                GanadorId: p.GanadorId!.Value,
                PuntajeGanador: p.PuntajeGanador!.Value,
                Jugadores: p.PartidaJugadores
                    .OrderBy(j => j.OrdenTurno)
                    .Select(j => new PartidaJugadorDto(
                        JugadorId: j.JugadorId,
                        Nombre: j.Jugador.Nombre,
                        Puntaje: j.Puntaje,
                        OrdenTurno: j.OrdenTurno
                    ))
                    .ToList()
            );

        public static MovimientoDto ADto(this Movimiento m)
            => new(
                PartidaId: m.PartidaId,
                JugadorId: m.JugadorId,
                IndicePrimero: m.IndicePrimero,
                IndiceSegundo: m.IndiceSegundo,
                FuePareja: m.FuePareja,
                CreadoUtc: m.CreadoUtc
            );
     }
}
