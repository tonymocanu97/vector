using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Slug).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);

            builder.HasIndex(c => c.Slug).IsUnique();

            var seededAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Category { Id = 1, Name = "Pro Kit", Slug = "pro-kit", Description = "Match-worn engineering, built for the arena and the streets.", CreatedAt = seededAt },
                new Category { Id = 2, Name = "Apparel", Slug = "apparel", Description = "Everyday gear for training and downtime.", CreatedAt = seededAt },
                new Category { Id = 3, Name = "Hardware", Slug = "hardware", Description = "Peripherals and gear used by our pro players.", CreatedAt = seededAt },
                new Category { Id = 4, Name = "Accessories", Slug = "accessories", Description = "Bags, headwear, and everyday carry.", CreatedAt = seededAt },
                new Category { Id = 5, Name = "Bundles", Slug = "bundles", Description = "Curated kit bundles at a better price.", CreatedAt = seededAt },
                new Category { Id = 6, Name = "Legacy", Slug = "legacy", Description = "Past season drops and archived collections.", CreatedAt = seededAt }
            );
        }
    }
}
