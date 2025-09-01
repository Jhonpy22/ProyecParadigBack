

namespace Domain.Entities
{
    public class Jugador
    {
        public int JugadorId { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;

        public ICollection<SalaJugador> Salas { get; set; } = new List<SalaJugador>();
        public ICollection<PartidaJugador> Partidas { get; set; } = new List<PartidaJugador>();
    }
}
