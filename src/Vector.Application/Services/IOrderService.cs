using Vector.Application.DTOs;

namespace Vector.Application.Services
{
    public interface IOrderService
    {
        // (null, error) -> 400 (cart empty or insufficient stock).
        Task<(OrderDto? Order, string? Error)> CheckoutAsync(
            int userId,
            CheckoutRequest request,
            CancellationToken ct = default);

        Task<IReadOnlyList<OrderDto>> GetOrdersAsync(int userId, CancellationToken ct = default);

        // Null -> 404 (not found, or belongs to a different user).
        Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId, CancellationToken ct = default);
    }
}
