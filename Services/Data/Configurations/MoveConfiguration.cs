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
    public class MoveConfiguration : IEntityTypeConfiguration<Move>
    {
        public void Configure(EntityTypeBuilder<Move> b)
        {
            b.HasKey(x => x.MoveId);
            b.HasIndex(x => x.GameId);

            b.Property(x => x.FirstIndex).IsRequired();
            b.Property(x => x.SecondIndex).IsRequired();
            b.Property(x => x.IsMatch).IsRequired();

            b.HasOne(x => x.Game)
             .WithMany(g => g.Moves)
             .HasForeignKey(x => x.GameId);

            b.HasOne(x => x.Player)
             .WithMany()
             .HasForeignKey(x => x.PlayerId);
        }
    }
}
