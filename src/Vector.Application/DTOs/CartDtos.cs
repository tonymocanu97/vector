using System.ComponentModel.DataAnnotations;

namespace Vector.Application.DTOs
{
    public record CartItemDto(
        int Id,
        int ProductId,
        string ProductName,
        string ProductImageUrl,
        decimal UnitPrice,
        int Quantity,
        decimal LineTotal);

    public record CartDto(int Id, List<CartItemDto> Items, decimal Total);

    public record AddCartItemRequest([Range(1, int.MaxValue)] int ProductId, [Range(1, int.MaxValue)] int Quantity);

    public record UpdateCartItemRequest([Range(1, int.MaxValue)] int Quantity);
}
