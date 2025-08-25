using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Services.Data.Configurations
{
    public class MatchPlayerConfiguration: IEntityTypeConfiguration<MatchPlayer>
    {
        public void Configure(EntityTypeBuilder<MatchPlayer> b)
        {
            b.HasKey(x => new { x.MatchId, x.PlayerId });
            b.Property(x => x.CumulativeScore).IsRequired();

            b.HasOne(x => x.Match)
             .WithMany(m => m.MatchPlayers)
             .HasForeignKey(x => x.MatchId);

            b.HasOne(x => x.Player)
             .WithMany(p => p.MatchPlayers)
             .HasForeignKey(x => x.PlayerId);
        }
    }
}
