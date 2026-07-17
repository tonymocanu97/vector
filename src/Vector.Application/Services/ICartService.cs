using Vector.Application.DTOs;

namespace Vector.Application.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId, CancellationToken ct = default);

        // (null, error) -> 400 (product not found or insufficient stock).
        Task<(CartDto? Cart, string? Error)> AddItemAsync(
            int userId,
            AddCartItemRequest request,
            CancellationToken ct = default);

        // (null, null) -> 404 (cart/item not found). (null, error) -> 400 (insufficient stock).
        Task<(CartDto? Cart, string? Error)> UpdateItemAsync(
            int userId,
            int productId,
            UpdateCartItemRequest request,
            CancellationToken ct = default);

        // Null -> 404 (cart/item not found).
        Task<CartDto?> RemoveItemAsync(int userId, int productId, CancellationToken ct = default);
    }
}
