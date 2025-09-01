

namespace Domain.Entities
{
    public class CartaTablero
    {
        public int CartaTableroId { get; set; }
        public int PartidaId { get; set; }
        public Partida Partida { get; set; } = null!;
        public int Indice { get; set; }    
        public int ClavePareja { get; set; }
        public bool EstaEmparejada { get; set; }
    }
}
