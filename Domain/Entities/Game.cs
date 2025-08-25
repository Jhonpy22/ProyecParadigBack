using Domain.Enums;

namespace Domain.Entities
{
    public class Game
    {
        public int GameId { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public GameStatus Status { get; set; } = GameStatus.InProgress;

        public int Rows { get; set; } = 20;
        public int Cols { get; set; } = 20;

        public int? CurrentPlayerId { get; set; }
        public Player? CurrentPlayer { get; set; }
        public int TurnNumber { get; set; } = 1;

        // buffer del turno: si tiene valor => OneRevealed
        public int? FirstFlipIndex { get; set; }

        public int MatchId { get; set; }

        public Match Match { get; set; } = null!;

        public int RoundNumber { get; set; }

        public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAtUtc { get; set; }

        public ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
        public ICollection<BoardCard> Board { get; set; } = new List<BoardCard>();
        public ICollection<Move> Moves { get; set; } = new List<Move>();
    }

}
