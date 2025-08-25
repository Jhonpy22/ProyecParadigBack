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
    public class RoomPlayerConfiguration : IEntityTypeConfiguration<RoomPlayer>
    {
        public void Configure(EntityTypeBuilder<RoomPlayer> b)
        {
            b.HasKey(x => new { x.RoomId, x.PlayerId });
            b.HasOne(x => x.Room).WithMany(r => r.Players).HasForeignKey(x => x.RoomId);
            b.HasOne(x => x.Player).WithMany(p => p.Rooms).HasForeignKey(x => x.PlayerId);
        }
    }
}
