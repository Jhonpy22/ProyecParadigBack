

namespace Domain.Entities
{
    public class Movimiento
    {
        public int MovimientoId { get; set; }
        public int PartidaId { get; set; }
        public Partida Partida { get; set; } = null!;
        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; } = null!;
        public int IndicePrimero { get; set; }
        public int IndiceSegundo { get; set; }
        public bool FuePareja { get; set; }
        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;
    }
}
