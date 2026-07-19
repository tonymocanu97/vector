using Microsoft.Extensions.Logging.Abstractions;
using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Application.Services;
using Vector.Domain.Entities;
using Vector.Domain.Enums;

namespace Vector.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<ICartRepository> _cartRepository = new();
        private readonly OrderService _sut;

        public OrderServiceTests()
        {
            _sut = new OrderService(_orderRepository.Object, _cartRepository.Object, NullLogger<OrderService>.Instance);
        }

        private static CheckoutRequest MakeCheckoutRequest() =>
            new("Jane Doe", "123 Main St", "Metropolis", "12345", "USA");

        private static Product MakeProduct(int id = 1, int stock = 10, decimal price = 20m, string name = "Widget") =>
            new() { Id = id, Name = name, Price = price, StockQuantity = stock, ImageUrl = "/images/widget.jpg" };

        [Fact]
        public async Task CheckoutAsync_NoCart_ReturnsError()
        {
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Cart?)null);

            var (order, error) = await _sut.CheckoutAsync(1, MakeCheckoutRequest());

            order.Should().BeNull();
            error.Should().Be("Your cart is empty.");
        }

        [Fact]
        public async Task CheckoutAsync_EmptyCart_ReturnsError()
        {
            var cart = new Cart { Id = 1, UserId = 1, Items = [] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

            var (order, error) = await _sut.CheckoutAsync(1, MakeCheckoutRequest());

            order.Should().BeNull();
            error.Should().Be("Your cart is empty.");
        }

        [Fact]
        public async Task CheckoutAsync_InsufficientStock_ReturnsErrorAndDoesNotPersist()
        {
            var product = MakeProduct(stock: 1);
            var cart = new Cart { Id = 1, UserId = 1, Items = [new CartItem { ProductId = 1, Product = product, Quantity = 5 }] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

            var (order, error) = await _sut.CheckoutAsync(1, MakeCheckoutRequest());

            order.Should().BeNull();
            error.Should().Contain("1");
            _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CheckoutAsync_ValidCart_CreatesOrderAndDecrementsStockAndClearsCart()
        {
            var product = MakeProduct(id: 1, stock: 10, price: 15m);
            var cartItem = new CartItem { ProductId = 1, Product = product, Quantity = 3 };
            var cart = new Cart { Id = 1, UserId = 1, Items = [cartItem] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

            var (order, error) = await _sut.CheckoutAsync(1, MakeCheckoutRequest());

            error.Should().BeNull();
            order.Should().NotBeNull();
            order!.TotalAmount.Should().Be(45m);
            order.Items.Should().ContainSingle(i => i.ProductId == 1 && i.Quantity == 3);
            order.Status.Should().Be(nameof(OrderStatus.Paid));

            product.StockQuantity.Should().Be(7);
            cart.Items.Should().BeEmpty();
            _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _orderRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsMappedOrders()
        {
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                Status = OrderStatus.Paid,
                TotalAmount = 30m,
                Items = [new OrderItem { ProductId = 1, ProductName = "Widget", UnitPrice = 10m, Quantity = 3 }]
            };
            _orderRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IReadOnlyList<Order>)[order]);

            var result = await _sut.GetOrdersAsync(1);

            result.Should().ContainSingle(o => o.Id == 1 && o.TotalAmount == 30m);
        }

        [Fact]
        public async Task GetOrderByIdAsync_OrderBelongsToDifferentUser_ReturnsNull()
        {
            var order = new Order { Id = 1, UserId = 2, Items = [] };
            _orderRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(order);

            var result = await _sut.GetOrderByIdAsync(1, 1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetOrderByIdAsync_NotFound_ReturnsNull()
        {
            _orderRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Order?)null);

            var result = await _sut.GetOrderByIdAsync(1, 99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetOrderByIdAsync_OwnedByUser_ReturnsDto()
        {
            var order = new Order { Id = 1, UserId = 1, TotalAmount = 20m, Items = [] };
            _orderRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(order);

            var result = await _sut.GetOrderByIdAsync(1, 1);

            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
        }
    }
}
