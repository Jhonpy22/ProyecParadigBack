

namespace Domain.Entities
{
  
    public class BoardCard
    {
        public int BoardCardId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int Index { get; set; } 
        public int PairKey { get; set; } 
        public bool IsMatched { get; set; }
    }
}
