


namespace Domain.Entities
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<RoomPlayer> Rooms { get; set; } = new List<RoomPlayer>();
        public ICollection<GamePlayer> Games { get; set; } = new List<GamePlayer>();
        public ICollection<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();
    }
}
