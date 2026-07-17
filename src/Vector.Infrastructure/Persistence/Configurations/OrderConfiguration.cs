using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(o => o.TotalAmount).HasPrecision(10, 2);

            builder.Property(o => o.ShippingFullName).IsRequired().HasMaxLength(200);
            builder.Property(o => o.ShippingAddressLine).IsRequired().HasMaxLength(300);
            builder.Property(o => o.ShippingCity).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ShippingPostalCode).IsRequired().HasMaxLength(20);
            builder.Property(o => o.ShippingCountry).IsRequired().HasMaxLength(100);

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.UserId);
        }
    }
}
