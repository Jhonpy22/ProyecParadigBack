using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Services.Data.Configurations
{
    public class SalaJugadorConfiguration: IEntityTypeConfiguration<SalaJugador>
    {
        public void Configure(EntityTypeBuilder<SalaJugador> b)
        {
            b.HasKey(x => new { x.SalaId, x.JugadorId });

            b.HasOne(x => x.Sala)
             .WithMany(r => r.Jugadores)
             .HasForeignKey(x => x.SalaId);

            b.HasOne(x => x.Jugador)
             .WithMany(p => p.Salas)
             .HasForeignKey(x => x.JugadorId);
        }
    }
}
