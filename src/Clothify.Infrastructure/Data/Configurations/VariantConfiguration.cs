using Clothify.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Data.Configurations;

public class VariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(v => v.Id);
        builder.HasIndex(v => v.SKU).IsUnique();
        builder.Property(v => v.SKU).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Size).HasMaxLength(10);
        builder.Property(v => v.Color).HasMaxLength(50);
        builder.Property(v => v.PriceOverride).HasPrecision(18, 2);

        builder.HasOne(v => v.Product)
            .WithMany(p => p.Variants)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
