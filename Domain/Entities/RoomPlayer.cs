

namespace Domain.Entities
{
    public class RoomPlayer
    {
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int? TurnOrder { get; set; } 
    }
}
