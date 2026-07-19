using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Application.Services;
using Vector.Domain.Entities;
using Vector.Domain.Enums;

namespace Vector.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICategoryRepository> _categoryRepository = new();
        private readonly ProductService _sut;

        public ProductServiceTests()
        {
            _sut = new ProductService(_productRepository.Object, _categoryRepository.Object);
        }

        private static Category MakeCategory(int id = 1) => new() { Id = id, Name = "Gear", Slug = "gear" };

        private static Product MakeProduct(int id = 1, Category? category = null) => new()
        {
            Id = id,
            Name = "Widget",
            Description = "A widget",
            Price = 10m,
            ImageUrl = "/images/widget.jpg",
            StockQuantity = 5,
            CategoryId = category?.Id ?? 1,
            Category = category ?? MakeCategory()
        };

        private static CreateProductRequest MakeCreateRequest(int categoryId = 1, string? tag = null) =>
            new("Widget", "A widget", 10m, "/images/widget.jpg", 5, tag, categoryId);

        private static UpdateProductRequest MakeUpdateRequest(int categoryId = 1) =>
            new("Widget Updated", "Updated description", 12m, "/images/widget2.jpg", 8, null, categoryId);

        [Fact]
        public async Task GetAllAsync_ReturnsMappedProducts()
        {
            _productRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IReadOnlyList<Product>)[MakeProduct()]);

            var result = await _sut.GetAllAsync();

            result.Should().ContainSingle(p => p.Id == 1 && p.CategorySlug == "gear");
        }

        [Fact]
        public async Task GetByCategorySlugAsync_ReturnsMappedProducts()
        {
            _productRepository.Setup(r => r.GetByCategorySlugAsync("gear", It.IsAny<CancellationToken>()))
                .ReturnsAsync((IReadOnlyList<Product>)[MakeProduct()]);

            var result = await _sut.GetByCategorySlugAsync("gear");

            result.Should().ContainSingle();
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNull()
        {
            _productRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var result = await _sut.GetByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_Found_ReturnsDto()
        {
            _productRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(MakeProduct());

            var result = await _sut.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
        }

        [Fact]
        public async Task CreateAsync_CategoryNotFound_ReturnsNull()
        {
            _categoryRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Category?)null);

            var result = await _sut.CreateAsync(MakeCreateRequest(categoryId: 99));

            result.Should().BeNull();
            _productRepository.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_PersistsAndReturnsDto()
        {
            var category = MakeCategory();
            _categoryRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(category);

            var result = await _sut.CreateAsync(MakeCreateRequest());

            result.Should().NotBeNull();
            result!.Name.Should().Be("Widget");
            result.CategorySlug.Should().Be("gear");
            _productRepository.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _productRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ValidTag_ParsesTagOntoProduct()
        {
            var category = MakeCategory();
            _categoryRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(category);

            var result = await _sut.CreateAsync(MakeCreateRequest(tag: nameof(ProductTag.New)));

            result!.Tag.Should().Be(nameof(ProductTag.New));
        }

        [Fact]
        public async Task CreateAsync_UnrecognizedTag_ParsesAsNull()
        {
            var category = MakeCategory();
            _categoryRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(category);

            var result = await _sut.CreateAsync(MakeCreateRequest(tag: "NotARealTag"));

            result!.Tag.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ProductNotFound_ReturnsNullTuple()
        {
            _productRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var (product, error) = await _sut.UpdateAsync(99, MakeUpdateRequest());

            product.Should().BeNull();
            error.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_CategoryNotFound_ReturnsError()
        {
            _productRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(MakeProduct());
            _categoryRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Category?)null);

            var (product, error) = await _sut.UpdateAsync(1, MakeUpdateRequest(categoryId: 99));

            product.Should().BeNull();
            error.Should().Contain("99");
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_UpdatesFieldsAndReturnsDto()
        {
            var existingProduct = MakeProduct();
            var newCategory = new Category { Id = 2, Name = "Apparel", Slug = "apparel" };
            _productRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingProduct);
            _categoryRepository.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(newCategory);

            var (product, error) = await _sut.UpdateAsync(1, MakeUpdateRequest(categoryId: 2));

            error.Should().BeNull();
            product.Should().NotBeNull();
            product!.Name.Should().Be("Widget Updated");
            product.Price.Should().Be(12m);
            product.CategorySlug.Should().Be("apparel");
            _productRepository.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
            _productRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _productRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var result = await _sut.DeleteAsync(99);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_Found_DeletesAndReturnsTrue()
        {
            var product = MakeProduct();
            _productRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(product);

            var result = await _sut.DeleteAsync(1);

            result.Should().BeTrue();
            _productRepository.Verify(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
            _productRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
