using Application.Common.Exceptions;
using Application.Contratos.Partidas;
using Application.Contratos.Turnos;
using Domain.Entities;

namespace Application.Mapeos
{
    public static class PartidasYTurnosComandosMapeos
    {
        
        public static Partida Construir(this IniciarPartidaRequest dto, Sala sala, int filas, int columnas, int duracionSegundos)
        {
            var p = new Partida
            {
                SalaId = sala.SalaId,
                Sala = sala,
                Dificultad = dto.Dificultad
            };

            p.EstablecerTamano(filas, columnas);        
            p.EstablecerTemporizador(duracionSegundos); 

            
            var primero = sala.Jugadores
                              .OrderBy(j => j.OrdenTurno ?? int.MaxValue)
                              .Select(j => j.JugadorId)
                              .FirstOrDefault();
            if (primero != 0)
                p.JugadorActualId = primero;

            return p;
        }

      
        public static Task AplicarAsync(this VoltearCartaRequest dto, Partida partida)
        {
            var jugador = partida.PartidaJugadores
            .FirstOrDefault(pj => pj.Jugador.Nombre == dto.NombreJugador)?.Jugador
            ?? throw new NotFoundException("Jugador no está en la partida.");

            return Domain.States.Turnos.FabricaEstadoTurno.From(partida)
                   .VoltearAsync(partida, jugador.JugadorId, dto.Indice);
        }

        public static PartidaJugadorDto ADto(this PartidaJugador pj)
            => new PartidaJugadorDto(
                JugadorId: pj.JugadorId,
                Nombre: pj.Jugador.Nombre,
                Puntaje: pj.Puntaje,
                OrdenTurno: pj.OrdenTurno
            );
    }
}
