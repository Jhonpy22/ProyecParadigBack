using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Data.Configurations
{
    public class GamePlayerConfiguration : IEntityTypeConfiguration<GamePlayer>
    {
        public void Configure(EntityTypeBuilder<GamePlayer> b)
        {
            b.HasKey(x => new { x.GameId, x.PlayerId });
            b.Property(x => x.TurnOrder).IsRequired();
            b.Property(x => x.Score).IsRequired();

            b.HasOne(x => x.Game)
             .WithMany(g => g.GamePlayers)
             .HasForeignKey(x => x.GameId);

            b.HasOne(x => x.Player)
             .WithMany(p => p.Games)
             .HasForeignKey(x => x.PlayerId);
        }
    }
}
