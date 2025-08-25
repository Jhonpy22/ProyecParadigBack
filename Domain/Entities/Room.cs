using Domain.Enums;



namespace Domain.Entities
{
    
    public class Room
    {
        public int RoomId { get; set; }
        public string JoinCode { get; set; } = null!;
        public RoomStatus Status { get; set; } = RoomStatus.Lobby;
        public int MaxPlayers { get; set; } = 4;
        public ICollection<RoomPlayer> Players { get; set; } = new List<RoomPlayer>();
        public Game? CurrentGame { get; set; }

        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
