using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Services.Data.Configurations
{
    public class BoardCardConfiguration : IEntityTypeConfiguration<BoardCard>
    {
        public void Configure(EntityTypeBuilder<BoardCard> b)
        {
            b.HasKey(x => x.BoardCardId);
            b.HasIndex(x => new { x.GameId, x.Index }).IsUnique();
            b.Property(x => x.Index).IsRequired();
            b.Property(x => x.PairKey).IsRequired();
            b.Property(x => x.IsMatched).IsRequired();
        }
    }
}
