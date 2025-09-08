using Application.Contratos.Salas;
using Domain.Entities;


namespace Application.Mapeos
{
    public static class SalasComandosMapeos
    {
        
        public static Sala Construir(this CrearSalaRequest dto, Jugador anfitrion)
        {
            var sala = new Sala
            {
                AnfitrionId = anfitrion.JugadorId,
                Anfitrion = anfitrion,
                MaxJugadores = dto.MaxJugadores,
            };

            if (!string.IsNullOrWhiteSpace(dto.CodigoIngreso))
                sala.EstablecerCodigoIngreso(dto.CodigoIngreso); 

            return sala;
        }

        
        public static RotarCodigoResponse ARespuestaRotacion(this Sala sala)
            => new RotarCodigoResponse(
                SalaId: sala.SalaId,
                CodigoIngreso: sala.CodigoIngreso
            );

        public static SalaJugadorDto ADto(this SalaJugador sj)
            => new SalaJugadorDto(
                JugadorId: sj.JugadorId,
                Nombre: sj.Jugador.Nombre,
                OrdenTurno: sj.OrdenTurno
            );
    }
}
