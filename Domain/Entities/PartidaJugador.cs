

namespace Domain.Entities
{
    public class PartidaJugador
    {
        public int PartidaId { get; set; }
        public Partida Partida { get; set; } = null!;
        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; } = null!;
        public int OrdenTurno { get; set; }
        public int Puntaje { get; set; }
    }
}
