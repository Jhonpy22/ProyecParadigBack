

namespace Domain.Entities
{

    public class Move
    {
        public int MoveId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        public int FirstIndex { get; set; }
        public int SecondIndex { get; set; }
        public bool IsMatch { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
