using Domain.Enums;


namespace Domain.Entities
{
    public class Match
    {
        public int MatchId { get; set; }

        public MatchStatus Status { get; set; } = MatchStatus.InProgress;

        public int CurrentRound { get; set; } = 1;

        public int TotalRounds { get; set; } = 3;

        public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? EndedAtUtc { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        
        public ICollection<Game> Games { get; set; } = new List<Game>();
        public ICollection<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();
    }
}
