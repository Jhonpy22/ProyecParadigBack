

namespace Domain.Entities
{
    public class GamePlayer
    {
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int TurnOrder { get; set; }
        public int Score { get; set; }
    }
}
