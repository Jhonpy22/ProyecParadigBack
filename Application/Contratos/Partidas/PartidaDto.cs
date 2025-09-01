    using Domain.Enums;

    namespace Application.Contratos.Partidas
    {
        public sealed record PartidaDto(
         int PartidaId,
         int SalaId,
         EstadoPartida Estado,
         Dificultad Dificultad,
         int Filas,
         int Columnas,
         int DuracionSegundos,
         DateTime IniciadaUtc,
         int? JugadorActualId,
         int NumeroTurno,
         int? GanadorId,
         int? PuntajeGanador,
         List<PartidaJugadorDto> Jugadores
        );
    }
