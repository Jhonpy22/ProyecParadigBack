using Domain.Enums;


namespace Domain.Entities
{
    public class Sala
    {
        public int SalaId { get; set; }
        public string CodigoIngreso { get; private set; } = null!;

        public int AnfitrionId { get; set; }
        public Jugador Anfitrion { get; set; } = null!;

        public void EstablecerCodigoIngreso(string? codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new InvalidOperationException("Código vacío.");

            var up = codigo.Trim().ToUpperInvariant();
            if (!System.Text.RegularExpressions.Regex.IsMatch(up, "^[A-Z0-9]{6,12}$"))
                throw new InvalidOperationException("Código inválido (A-Z/0-9, 6–12).");

            if (Estado != EstadoSala.Lobby)
                throw new InvalidOperationException("Solo puede cambiarse el código en Lobby.");

            CodigoIngreso = up;
        }

        public EstadoSala Estado { get; set; } = EstadoSala.Lobby;
        public int MaxJugadores { get; set; } = 4;

        public ICollection<SalaJugador> Jugadores { get; set; } = new List<SalaJugador>();

        public ICollection<Partida> Partidas { get; set; } = new List<Partida>();

        public int? PartidaActualId { get; set; }
        public Partida? PartidaActual { get; set; }
       
    }
}
