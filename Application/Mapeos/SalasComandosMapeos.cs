using Application.Contratos.Salas;
using Domain.Entities;


namespace Application.Mapeos
{
    public static class SalasComandosMapeos
    {
        // Crear Sala desde request, respetando reglas de dominio (código solo en Lobby)
        public static Sala Construir(this CrearSalaRequest dto, Jugador anfitrion)
        {
            var sala = new Sala
            {
                AnfitrionId = anfitrion.JugadorId,
                Anfitrion = anfitrion,
                MaxJugadores = dto.MaxJugadores,
            };

            if (!string.IsNullOrWhiteSpace(dto.CodigoIngreso))
                sala.EstablecerCodigoIngreso(dto.CodigoIngreso); // valida formato y estado

            return sala;
        }

        // Sala → RotarCodigoResponse (convenience para devolver el nuevo código)
        public static RotarCodigoResponse ARespuestaRotacion(this Sala sala)
            => new RotarCodigoResponse(
                SalaId: sala.SalaId,
                CodigoIngreso: sala.CodigoIngreso
            );

        // SalaJugador → SalaJugadorDto (útil si alguna vez lo necesitas directo)
        public static SalaJugadorDto ADto(this SalaJugador sj)
            => new SalaJugadorDto(
                JugadorId: sj.JugadorId,
                Nombre: sj.Jugador.Nombre,
                OrdenTurno: sj.OrdenTurno
            );
    }
}
