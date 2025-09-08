using Domain.Enums;


namespace Domain.Entities
{
    public class Partida
    {
        public int PartidaId { get; set; }

        public int SalaId { get; set; }
        public Sala Sala { get; set; } = null!;

        
        public int? GanadorId { get; set; }
        public Jugador? Ganador { get; set; }
        public int? PuntajeGanador { get; set; }

        public EstadoPartida Estado { get; set; } = EstadoPartida.EnProgreso;

        public Dificultad Dificultad { get; set; }

        public int Filas { get; private set; }
        public int Columnas { get; private set; }

        public void EstablecerTamano(int filas, int columnas)
        {
            if (filas <= 0 || columnas <= 0 || (filas * columnas) % 2 != 0)
                throw new InvalidOperationException("Dimensiones inválidas (filas*columnas debe ser par).");
            Filas = filas;
            Columnas = columnas;
        }

        public int? JugadorActualId { get; set; }
        public Jugador? JugadorActual { get; set; }
        public int NumeroTurno { get; set; } = 1;
        public int? IndicePrimerVolteo { get; set; }

        public int DuracionSegundos { get; private set; }
        public DateTime IniciadaUtc { get; private set; } = DateTime.UtcNow;

        public void EstablecerTemporizador(int duracionSegundos)
        {
            if (duracionSegundos <= 0) throw new InvalidOperationException("La duración debe ser > 0.");
            DuracionSegundos = duracionSegundos;
           
        }

        public int PuntosPorPareja { get; private set; } = 1;

        public void EstablecerPuntosPorPareja(int puntos)
        {
            if (puntos <= 0) throw new InvalidOperationException("Puntos por pareja debe ser > 0.");
            PuntosPorPareja = puntos;
        }


        public bool Expirada(DateTime ahoraUtc)
            => ahoraUtc >= IniciadaUtc.AddSeconds(DuracionSegundos);

        public DateTime? FinalizadaUtc { get; set; }

        public ICollection<PartidaJugador> PartidaJugadores { get; set; } = new List<PartidaJugador>();
        public ICollection<CartaTablero> Tablero { get; set; } = new List<CartaTablero>();
        public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }

}

