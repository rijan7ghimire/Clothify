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
            // Top-level gender/type categories
            new Category { Id = 1, Name = "Boys", Slug = "boys", ImageUrl = "/images/categories/boys.jpg" },
            new Category { Id = 2, Name = "Girls", Slug = "girls", ImageUrl = "/images/categories/girls.jpg" },
            new Category { Id = 3, Name = "Men", Slug = "men", ImageUrl = "/images/categories/men.jpg" },
            new Category { Id = 4, Name = "Women", Slug = "women", ImageUrl = "/images/categories/women.jpg" },

            // Boys subcategories
            new Category { Id = 10, Name = "Boys Topwear", Slug = "boys-topwear", ParentCategoryId = 1 },
            new Category { Id = 11, Name = "Boys Bottomwear", Slug = "boys-bottomwear", ParentCategoryId = 1 },
            new Category { Id = 12, Name = "Boys Apparel Sets", Slug = "boys-apparel-sets", ParentCategoryId = 1 },
            new Category { Id = 13, Name = "Boys Innerwear", Slug = "boys-innerwear", ParentCategoryId = 1 },

            // Girls subcategories
            new Category { Id = 20, Name = "Girls Topwear", Slug = "girls-topwear", ParentCategoryId = 2 },
            new Category { Id = 21, Name = "Girls Bottomwear", Slug = "girls-bottomwear", ParentCategoryId = 2 },
            new Category { Id = 22, Name = "Girls Dresses", Slug = "girls-dresses", ParentCategoryId = 2 },
            new Category { Id = 23, Name = "Girls Apparel Sets", Slug = "girls-apparel-sets", ParentCategoryId = 2 },
            new Category { Id = 24, Name = "Girls Innerwear", Slug = "girls-innerwear", ParentCategoryId = 2 },

            // Men Footwear subcategories
            new Category { Id = 30, Name = "Men Shoes", Slug = "men-shoes", ParentCategoryId = 3 },
            new Category { Id = 31, Name = "Men Sandals", Slug = "men-sandals", ParentCategoryId = 3 },
            new Category { Id = 32, Name = "Men Flip Flops", Slug = "men-flip-flops", ParentCategoryId = 3 },

            // Women Footwear subcategories
            new Category { Id = 40, Name = "Women Shoes", Slug = "women-shoes", ParentCategoryId = 4 },
            new Category { Id = 41, Name = "Women Sandals", Slug = "women-sandals", ParentCategoryId = 4 },
            new Category { Id = 42, Name = "Women Flip Flops", Slug = "women-flip-flops", ParentCategoryId = 4 },
            new Category { Id = 43, Name = "Women Heels", Slug = "women-heels", ParentCategoryId = 4 }
        );
    }
}
