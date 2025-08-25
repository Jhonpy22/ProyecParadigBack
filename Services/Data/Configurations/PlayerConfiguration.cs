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
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> b)
        {
            b.HasKey(x => x.PlayerId);
            b.Property(x => x.Name).HasMaxLength(60).IsRequired();
            // Relaciones con RoomPlayer/GamePlayer se configuran en esas clases
        }
    }
}
