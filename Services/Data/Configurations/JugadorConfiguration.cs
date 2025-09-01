using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class JugadorConfiguration: IEntityTypeConfiguration<Jugador>
    {
        public void Configure(EntityTypeBuilder<Jugador> b)
        {
            b.HasKey(x => x.JugadorId);
            b.Property(x => x.Nombre).HasMaxLength(60).IsRequired();
        }
    }
}
