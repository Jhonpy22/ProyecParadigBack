using Application.Contratos.Partidas;
using Application.Contratos.Turnos;
using Domain.Entities;

namespace Application.Mapeos
{
    public static class PartidasYTurnosComandosMapeos
    {
        // Construye Partida desde el request + parámetros de tamaño y temporizador
        public static Partida Construir(this IniciarPartidaRequest dto, Sala sala, int filas, int columnas, int duracionSegundos)
        {
            var p = new Partida
            {
                SalaId = sala.SalaId,
                Sala = sala,
                Dificultad = dto.Dificultad
            };

            p.EstablecerTamano(filas, columnas);        // valida filas*columnas par
            p.EstablecerTemporizador(duracionSegundos); // valida duración > 0

            // Primer jugador según orden de la sala (si ya está cargado)
            var primero = sala.Jugadores
                              .OrderBy(j => j.OrdenTurno ?? int.MaxValue)
                              .Select(j => j.JugadorId)
                              .FirstOrDefault();
            if (primero != 0)
                p.JugadorActualId = primero;

            return p;
        }

        // Aplica un VoltearCartaRequest a la Partida (ejecuta la regla de dominio)
        public static Task AplicarAsync(this VoltearCartaRequest dto, Partida partida)
            => Domain.States.Turnos.FabricaEstadoTurno.From(partida)
                   .VoltearAsync(partida, dto.JugadorId, dto.Indice);

        // PartidaJugador → PartidaJugadorDto (por si lo necesitas directo)
        public static PartidaJugadorDto ADto(this PartidaJugador pj)
            => new PartidaJugadorDto(
                JugadorId: pj.JugadorId,
                Nombre: pj.Jugador.Nombre,
                Puntaje: pj.Puntaje,
                OrdenTurno: pj.OrdenTurno
            );
    }
}
