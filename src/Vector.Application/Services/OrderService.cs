using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;
using Vector.Domain.Enums;

namespace Vector.Application.Services
{
    public class OrderService(IOrderRepository orderRepository, ICartRepository cartRepository) : IOrderService
    {
        // No real payment gateway is wired up (out of scope for this project) - checkout
        // validates stock, snapshots the order, and marks it Paid immediately to simulate
        // a successful payment.
        public async Task<(OrderDto? Order, string? Error)> CheckoutAsync(
            int userId,
            CheckoutRequest request,
            CancellationToken ct = default)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId, ct);

            if (cart is null || cart.Items.Count == 0)
            {
                return (null, "Your cart is empty.");
            }

            foreach (var item in cart.Items)
            {
                if (item.Quantity > item.Product.StockQuantity)
                {
                    return (null, $"Only {item.Product.StockQuantity} unit(s) of '{item.Product.Name}' are available.");
                }
            }

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Paid,
                ShippingFullName = request.ShippingFullName.Trim(),
                ShippingAddressLine = request.ShippingAddressLine.Trim(),
                ShippingCity = request.ShippingCity.Trim(),
                ShippingPostalCode = request.ShippingPostalCode.Trim(),
                ShippingCountry = request.ShippingCountry.Trim(),
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    UnitPrice = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
            order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);

            foreach (var item in cart.Items)
            {
                item.Product.StockQuantity -= item.Quantity;
            }

            cart.Items.Clear();

            await orderRepository.AddAsync(order, ct);
            // Product/Cart repos share the same scoped DbContext, so this one call
            // commits the stock decrement and cart clear together with the new order.
            await orderRepository.SaveChangesAsync(ct);

            return (ToDto(order), null);
        }

        public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync(int userId, CancellationToken ct = default)
        {
            var orders = await orderRepository.GetByUserIdAsync(userId, ct);
            return orders.Select(ToDto).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId, CancellationToken ct = default)
        {
            var order = await orderRepository.GetByIdAsync(orderId, ct);
            return order is null || order.UserId != userId ? null : ToDto(order);
        }

        private static OrderDto ToDto(Order order)
        {
            var items = order.Items
                .Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity))
                .ToList();

            return new OrderDto(order.Id, order.Status.ToString(), order.TotalAmount, order.CreatedAt, items);
        }
    }
}
