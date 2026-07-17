using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Application.Services
{
    public class CartService(ICartRepository cartRepository, IProductRepository productRepository) : ICartService
    {
        public async Task<CartDto> GetCartAsync(int userId, CancellationToken ct = default)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId, ct);
            return cart is null ? new CartDto(0, [], 0m) : ToDto(cart);
        }

        public async Task<(CartDto? Cart, string? Error)> AddItemAsync(
            int userId,
            AddCartItemRequest request,
            CancellationToken ct = default)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId, ct);
            if (product is null)
            {
                return (null, $"Product with id '{request.ProductId}' was not found.");
            }

            var cart = await GetOrCreateCartAsync(userId, ct);
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            var requestedQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

            var stockError = CheckStock(product, requestedQuantity);
            if (stockError is not null)
            {
                return (null, stockError);
            }

            if (existingItem is not null)
            {
                existingItem.Quantity = requestedQuantity;
            }
            else
            {
                cart.Items.Add(new CartItem { ProductId = product.Id, Product = product, Quantity = request.Quantity });
            }

            await cartRepository.SaveChangesAsync(ct);

            return (ToDto(cart), null);
        }

        public async Task<(CartDto? Cart, string? Error)> UpdateItemAsync(
            int userId,
            int productId,
            UpdateCartItemRequest request,
            CancellationToken ct = default)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId, ct);
            var item = cart?.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cart is null || item is null)
            {
                return (null, null);
            }

            var stockError = CheckStock(item.Product, request.Quantity);
            if (stockError is not null)
            {
                return (null, stockError);
            }

            item.Quantity = request.Quantity;

            await cartRepository.SaveChangesAsync(ct);

            return (ToDto(cart), null);
        }

        public async Task<CartDto?> RemoveItemAsync(int userId, int productId, CancellationToken ct = default)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId, ct);
            var item = cart?.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cart is null || item is null)
            {
                return null;
            }

            cart.Items.Remove(item);

            await cartRepository.SaveChangesAsync(ct);

            return ToDto(cart);
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId, CancellationToken ct)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId, ct);
            if (cart is not null)
            {
                return cart;
            }

            cart = new Cart { UserId = userId };
            await cartRepository.AddAsync(cart, ct);

            return cart;
        }

        private static string? CheckStock(Product product, int requestedQuantity) =>
            requestedQuantity > product.StockQuantity
                ? $"Only {product.StockQuantity} unit(s) of '{product.Name}' are available."
                : null;

        private static CartDto ToDto(Cart cart)
        {
            var items = cart.Items
                .Select(i => new CartItemDto(
                    i.Id,
                    i.ProductId,
                    i.Product.Name,
                    i.Product.ImageUrl,
                    i.Product.Price,
                    i.Quantity,
                    i.Product.Price * i.Quantity))
                .ToList();

            return new CartDto(cart.Id, items, items.Sum(i => i.LineTotal));
        }
    }
}
