using System.ComponentModel.DataAnnotations;

namespace Vector.Application.DTOs
{
    public record ProductDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        string ImageUrl,
        int StockQuantity,
        string? Tag,
        int CategoryId,
        string CategoryName,
        string CategorySlug);

    public record CreateProductRequest(
        [Required] string Name,
        string Description,
        [Range(0, double.MaxValue)] decimal Price,
        string ImageUrl,
        [Range(0, int.MaxValue)] int StockQuantity,
        string? Tag,
        [Range(1, int.MaxValue)] int CategoryId);

    public record UpdateProductRequest(
        [Required] string Name,
        string Description,
        [Range(0, double.MaxValue)] decimal Price,
        string ImageUrl,
        [Range(0, int.MaxValue)] int StockQuantity,
        string? Tag,
        [Range(1, int.MaxValue)] int CategoryId);
}
