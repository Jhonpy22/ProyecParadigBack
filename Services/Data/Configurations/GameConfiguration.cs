using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> b)
        {
            b.HasKey(x => x.GameId);
            b.Property(x => x.Status).HasConversion<int>(); // enum -> int

            b.HasOne(x => x.CurrentPlayer)
             .WithMany()
             .HasForeignKey(x => x.CurrentPlayerId)
             .OnDelete(DeleteBehavior.NoAction);

            b.Property(x => x.RoundNumber).IsRequired();
            b.HasOne(x => x.Match)
             .WithMany(m => m.Games)
             .HasForeignKey(x => x.MatchId);


            b.HasMany(x => x.Board).WithOne(c => c.Game).HasForeignKey(c => c.GameId);
            b.HasMany(x => x.GamePlayers).WithOne(gp => gp.Game).HasForeignKey(gp => gp.GameId);
            b.HasMany(x => x.Moves).WithOne(m => m.Game).HasForeignKey(m => m.GameId);
        }
    }
}
