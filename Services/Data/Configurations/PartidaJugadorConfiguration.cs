using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class PartidaJugadorConfiguration: IEntityTypeConfiguration<PartidaJugador>
    {
        public void Configure(EntityTypeBuilder<PartidaJugador> b)
        {
            b.HasKey(x => new { x.PartidaId, x.JugadorId });

            b.Property(x => x.OrdenTurno).IsRequired();
            b.Property(x => x.Puntaje).IsRequired();

            b.HasOne(x => x.Partida)
             .WithMany(p => p.PartidaJugadores)
             .HasForeignKey(x => x.PartidaId);

            b.HasOne(x => x.Jugador)
             .WithMany(j => j.Partidas)
             .HasForeignKey(x => x.JugadorId);
        }
    }
}
