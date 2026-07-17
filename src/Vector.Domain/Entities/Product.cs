using Vector.Domain.Enums;

namespace Vector.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public ProductTag? Tag { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
