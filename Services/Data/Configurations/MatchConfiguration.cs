using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Services.Data.Configurations
{
    public class MatchConfiguration: IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> b)
        {
            b.HasKey(x => x.MatchId);
            b.Property(x => x.Status).HasConversion<int>();

            b.HasOne(x => x.Room)
             .WithMany(x => x.Matches)
             .HasForeignKey(x => x.RoomId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.Games)
             .WithOne(g => g.Match)
             .HasForeignKey(g => g.MatchId);

            b.HasMany(x => x.MatchPlayers)
             .WithOne(mp => mp.Match)
             .HasForeignKey(mp => mp.MatchId);
        }
    }
}
