using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class MovimientoConfiguration: IEntityTypeConfiguration<Movimiento>
    {
        public void Configure(EntityTypeBuilder<Movimiento> b)
        {
            b.HasKey(x => x.MovimientoId);
            b.HasIndex(x => x.PartidaId);

            b.Property(x => x.IndicePrimero).IsRequired();
            b.Property(x => x.IndiceSegundo).IsRequired();
            b.Property(x => x.FuePareja).IsRequired();

            b.HasOne(x => x.Partida)
             .WithMany(p => p.Movimientos)
             .HasForeignKey(x => x.PartidaId);

            b.HasOne(x => x.Jugador)
             .WithMany()
             .HasForeignKey(x => x.JugadorId);
        }
    }
}
