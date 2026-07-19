using Microsoft.Extensions.Logging.Abstractions;
using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Application.Services;
using Vector.Domain.Entities;

namespace Vector.UnitTests.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepository = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly CartService _sut;

        public CartServiceTests()
        {
            _sut = new CartService(_cartRepository.Object, _productRepository.Object, NullLogger<CartService>.Instance);
        }

        private static Product MakeProduct(int id = 1, int stock = 10, decimal price = 25m, string name = "Widget") =>
            new()
            {
                Id = id,
                Name = name,
                Price = price,
                StockQuantity = stock,
                ImageUrl = "/images/widget.jpg",
                Category = new Category { Id = 1, Name = "Gear", Slug = "gear" }
            };

        [Fact]
        public async Task GetCartAsync_NoCart_ReturnsEmptyCart()
        {
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Cart?)null);

            var result = await _sut.GetCartAsync(1);

            result.Items.Should().BeEmpty();
            result.Total.Should().Be(0m);
        }

        [Fact]
        public async Task GetCartAsync_ExistingCart_MapsItemsAndTotal()
        {
            var product = MakeProduct(price: 10m);
            var cart = new Cart { Id = 5, UserId = 1, Items = [new CartItem { ProductId = 1, Product = product, Quantity = 3 }] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

            var result = await _sut.GetCartAsync(1);

            result.Items.Should().ContainSingle();
            result.Total.Should().Be(30m);
        }

        [Fact]
        public async Task AddItemAsync_ProductNotFound_ReturnsError()
        {
            _productRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var (cart, error) = await _sut.AddItemAsync(1, new AddCartItemRequest(99, 1));

            cart.Should().BeNull();
            error.Should().Contain("99");
        }

        [Fact]
        public async Task AddItemAsync_ExceedsStock_ReturnsError()
        {
            var product = MakeProduct(stock: 2);
            _productRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Cart?)null);

            var (cart, error) = await _sut.AddItemAsync(1, new AddCartItemRequest(1, 3));

            cart.Should().BeNull();
            error.Should().Contain("2");
            _cartRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddItemAsync_NoExistingCart_CreatesCartAndAddsItem()
        {
            var product = MakeProduct(stock: 10);
            _productRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Cart?)null);

            var (cart, error) = await _sut.AddItemAsync(1, new AddCartItemRequest(1, 2));

            error.Should().BeNull();
            cart.Should().NotBeNull();
            cart!.Items.Should().ContainSingle(i => i.ProductId == 1 && i.Quantity == 2);
            _cartRepository.Verify(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
            _cartRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ExistingItemInCart_IncrementsQuantity()
        {
            var product = MakeProduct(stock: 10);
            var existingCart = new Cart { Id = 5, UserId = 1, Items = [new CartItem { ProductId = 1, Product = product, Quantity = 2 }] };
            _productRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingCart);

            var (cart, error) = await _sut.AddItemAsync(1, new AddCartItemRequest(1, 3));

            error.Should().BeNull();
            cart!.Items.Should().ContainSingle(i => i.Quantity == 5);
            _cartRepository.Verify(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateItemAsync_CartMissing_ReturnsNullTuple()
        {
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Cart?)null);

            var (cart, error) = await _sut.UpdateItemAsync(1, 1, new UpdateCartItemRequest(2));

            cart.Should().BeNull();
            error.Should().BeNull();
        }

        [Fact]
        public async Task UpdateItemAsync_ItemNotInCart_ReturnsNullTuple()
        {
            var existingCart = new Cart { Id = 5, UserId = 1, Items = [] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingCart);

            var (cart, error) = await _sut.UpdateItemAsync(1, 1, new UpdateCartItemRequest(2));

            cart.Should().BeNull();
            error.Should().BeNull();
        }

        [Fact]
        public async Task UpdateItemAsync_ExceedsStock_ReturnsError()
        {
            var product = MakeProduct(stock: 2);
            var existingCart = new Cart { Id = 5, UserId = 1, Items = [new CartItem { ProductId = 1, Product = product, Quantity = 1 }] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingCart);

            var (cart, error) = await _sut.UpdateItemAsync(1, 1, new UpdateCartItemRequest(5));

            cart.Should().BeNull();
            error.Should().Contain("2");
        }

        [Fact]
        public async Task UpdateItemAsync_ValidQuantity_UpdatesItem()
        {
            var product = MakeProduct(stock: 10);
            var existingCart = new Cart { Id = 5, UserId = 1, Items = [new CartItem { ProductId = 1, Product = product, Quantity = 1 }] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingCart);

            var (cart, error) = await _sut.UpdateItemAsync(1, 1, new UpdateCartItemRequest(4));

            error.Should().BeNull();
            cart!.Items.Should().ContainSingle(i => i.Quantity == 4);
            _cartRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoveItemAsync_CartMissing_ReturnsNull()
        {
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Cart?)null);

            var result = await _sut.RemoveItemAsync(1, 1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveItemAsync_ValidItem_RemovesFromCart()
        {
            var product = MakeProduct();
            var existingCart = new Cart { Id = 5, UserId = 1, Items = [new CartItem { ProductId = 1, Product = product, Quantity = 1 }] };
            _cartRepository.Setup(r => r.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingCart);

            var result = await _sut.RemoveItemAsync(1, 1);

            result.Should().NotBeNull();
            result!.Items.Should().BeEmpty();
            _cartRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
