using Clothify.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(100).IsRequired();

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Category { Id = 1, Name = "Men", Slug = "men" },
            new Category { Id = 2, Name = "Women", Slug = "women" },
            new Category { Id = 3, Name = "Kids", Slug = "kids" },
            new Category { Id = 4, Name = "Accessories", Slug = "accessories" },
            new Category { Id = 5, Name = "Sale", Slug = "sale" },
            new Category { Id = 6, Name = "Tops", Slug = "tops", ParentCategoryId = 2 },
            new Category { Id = 7, Name = "Bottoms", Slug = "bottoms", ParentCategoryId = 2 },
            new Category { Id = 8, Name = "Dresses", Slug = "dresses", ParentCategoryId = 2 },
            new Category { Id = 9, Name = "Outerwear", Slug = "outerwear", ParentCategoryId = 1 },
            new Category { Id = 10, Name = "Shoes", Slug = "shoes" }
        );
    }
}
