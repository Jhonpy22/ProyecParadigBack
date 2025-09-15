using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class SalaConfiguration: IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> b)
        {
            b.HasKey(x => x.SalaId);
            b.Property(x => x.Estado).HasConversion<int>();
            b.Property(x => x.CodigoIngreso).HasMaxLength(12).IsRequired();
            b.HasIndex(x => x.CodigoIngreso).IsUnique();

            b.HasMany(x => x.Partidas)
            .WithOne(p => p.Sala)
            .HasForeignKey(p => p.SalaId)
            .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.PartidaActual)
             .WithMany()
             .HasForeignKey(p => p.PartidaActualId)
             .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(x => x.Anfitrion)
            .WithMany()
            .HasForeignKey(x => x.AnfitrionId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
