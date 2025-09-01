using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class CartaTableroConfiguration: IEntityTypeConfiguration<CartaTablero>
    {
        public void Configure(EntityTypeBuilder<CartaTablero> b)
        {
            b.HasKey(x => x.CartaTableroId);
            b.HasIndex(x => new { x.PartidaId, x.Indice }).IsUnique();
            b.Property(x => x.Indice).IsRequired();
            b.Property(x => x.ClavePareja).IsRequired();
            b.Property(x => x.EstaEmparejada).IsRequired();
        }
    }
}
