using System.ComponentModel.DataAnnotations;

namespace Vector.Application.DTOs
{
    public record OrderItemDto(int ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal LineTotal);

    public record OrderDto(int Id, string Status, decimal TotalAmount, DateTime CreatedAt, List<OrderItemDto> Items);

    public record CheckoutRequest(
        [Required] string ShippingFullName,
        [Required] string ShippingAddressLine,
        [Required] string ShippingCity,
        [Required] string ShippingPostalCode,
        [Required] string ShippingCountry);
}
