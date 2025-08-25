using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<RoomPlayer> RoomPlayers => Set<RoomPlayer>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GamePlayer> GamePlayers => Set<GamePlayer>();
    public DbSet<BoardCard> BoardCards => Set<BoardCard>();
    public DbSet<Move> Moves => Set<Move>();

    public DbSet<Match> Matches => Set<Match>();

    public DbSet<MatchPlayer> MatchPlayers => Set<MatchPlayer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
