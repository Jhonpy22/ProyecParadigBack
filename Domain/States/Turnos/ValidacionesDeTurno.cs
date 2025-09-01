using Domain.Entities;
using Domain.Enums;
using System.Linq;


namespace Domain.States.Turnos
{
    internal static class ValidacionesDeTurno
    {
        public static void AsegurarTurnoValido(Partida p, int jugadorId)
        {
            if (p.Estado != EstadoPartida.EnProgreso)
                throw new InvalidOperationException("La partida no está en progreso.");

            if (!p.PartidaJugadores.Any(x => x.JugadorId == jugadorId))
                throw new InvalidOperationException("No perteneces a esta partida.");

            if (!p.JugadorActualId.HasValue)
                throw new InvalidOperationException("Estado inconsistente: no hay jugador actual asignado.");

            if (p.JugadorActualId.Value != jugadorId)
                throw new InvalidOperationException("No es tu turno.");
        }
    }
}
