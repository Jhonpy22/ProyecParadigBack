using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Services.Data.Configurations
{
    public class PartidaConfiguration : IEntityTypeConfiguration<Partida>
    {
        public void Configure(EntityTypeBuilder<Partida> b)
        {
            b.HasKey(x => x.PartidaId);

            b.Property(x => x.Estado).HasConversion<int>();
            b.Property(x => x.Dificultad).HasConversion<int>();

            b.Property(x => x.Filas).IsRequired();
            b.Property(x => x.Columnas).IsRequired();

            b.Property(x => x.DuracionSegundos).IsRequired();
            b.Property(x => x.IniciadaUtc).IsRequired();


            b.HasOne(x => x.JugadorActual)
             .WithMany()
             .HasForeignKey(x => x.JugadorActualId)
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(x => x.Ganador)
             .WithMany()
             .HasForeignKey(x => x.GanadorId)
             .OnDelete(DeleteBehavior.NoAction);

            b.HasMany(x => x.Tablero).WithOne(c => c.Partida).HasForeignKey(c => c.PartidaId);

            b.HasMany(x => x.PartidaJugadores).WithOne(pj => pj.Partida).HasForeignKey(pj => pj.PartidaId);

            b.HasMany(x => x.Movimientos).WithOne(m => m.Partida).HasForeignKey(m => m.PartidaId);
        }
    }
}
