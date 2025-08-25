using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> b)
        {
            b.HasKey(x => x.RoomId);
            b.Property(x => x.Status).HasConversion<int>();
            b.Property(x => x.JoinCode).HasMaxLength(12).IsRequired();
            b.HasIndex(x => x.JoinCode).IsUnique();
            b.HasOne(x => x.CurrentGame)
             .WithOne(g => g.Room)
             .HasForeignKey<Game>(g => g.RoomId);
        }
    }
}
