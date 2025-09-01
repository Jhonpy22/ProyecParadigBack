
namespace Domain.Entities
{
    public class SalaJugador
    {
        public int SalaId { get; set; }
        public Sala Sala { get; set; } = null!;
        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; } = null!;
        public int? OrdenTurno { get; set; }
    }
}
