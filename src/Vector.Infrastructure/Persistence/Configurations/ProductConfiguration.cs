using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vector.Domain.Entities;
using Vector.Domain.Enums;

namespace Vector.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).HasMaxLength(2000);
            builder.Property(p => p.Price).HasPrecision(10, 2);
            builder.Property(p => p.ImageUrl).HasMaxLength(500);
            builder.Property(p => p.Tag).HasConversion<string>().HasMaxLength(20);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.CategoryId);

            var seededAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Product { Id = 1, Name = "Pro Kit Jersey 2026", Description = "Official match-worn jersey for the 2026 season.", Price = 74.99m, ImageUrl = "/images/product-jersey.jpg", StockQuantity = 50, Tag = ProductTag.New, CategoryId = 1, CreatedAt = seededAt },
                new Product { Id = 2, Name = "Core Blackout Hoodie", Description = "Heavyweight fleece hoodie in blackout colorway.", Price = 89.99m, ImageUrl = "/images/product-hoodie.jpg", StockQuantity = 50, Tag = ProductTag.Bestseller, CategoryId = 2, CreatedAt = seededAt },
                new Product { Id = 3, Name = "Arena Mousepad XL", Description = "Extended surface mousepad used by our pro roster.", Price = 34.99m, ImageUrl = "/images/product-mousepad.jpg", StockQuantity = 50, Tag = null, CategoryId = 3, CreatedAt = seededAt },
                new Product { Id = 4, Name = "Emblem Snap-Back Cap", Description = "Structured snap-back with embroidered emblem.", Price = 29.99m, ImageUrl = "/images/product-cap.jpg", StockQuantity = 50, Tag = null, CategoryId = 2, CreatedAt = seededAt },
                new Product { Id = 5, Name = "Signal Graphic Tee", Description = "Lightweight cotton tee with graphic print.", Price = 39.99m, ImageUrl = "/images/product-tee.jpg", StockQuantity = 50, Tag = ProductTag.New, CategoryId = 2, CreatedAt = seededAt },
                new Product { Id = 6, Name = "Pro Track Joggers", Description = "Tapered joggers built for training and travel.", Price = 84.99m, ImageUrl = "/images/product-joggers.jpg", StockQuantity = 50, Tag = null, CategoryId = 1, CreatedAt = seededAt },
                new Product { Id = 7, Name = "Loadout Backpack 22L", Description = "Padded backpack with dedicated laptop sleeve.", Price = 119.99m, ImageUrl = "/images/product-backpack.jpg", StockQuantity = 50, Tag = ProductTag.Limited, CategoryId = 4, CreatedAt = seededAt },
                new Product { Id = 8, Name = "Team Zip Hoodie", Description = "Full-zip hoodie with embroidered team crest.", Price = 99.99m, ImageUrl = "/images/product-hoodie.jpg", StockQuantity = 50, Tag = null, CategoryId = 2, CreatedAt = seededAt }
            );
        }
    }
}
