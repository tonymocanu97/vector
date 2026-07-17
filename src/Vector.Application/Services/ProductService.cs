using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;
using Vector.Domain.Enums;

namespace Vector.Application.Services
{
    public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        : IProductService
    {
        public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct = default)
        {
            var products = await productRepository.GetAllAsync(ct);
            return products.Select(ToDto).ToList();
        }

        public async Task<IReadOnlyList<ProductDto>> GetByCategorySlugAsync(
            string categorySlug,
            CancellationToken ct = default)
        {
            var products = await productRepository.GetByCategorySlugAsync(categorySlug, ct);
            return products.Select(ToDto).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var product = await productRepository.GetByIdAsync(id, ct);
            return product is null ? null : ToDto(product);
        }

        public async Task<ProductDto?> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId, ct);
            if (category is null)
            {
                return null;
            }

            var product = new Product
            {
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                StockQuantity = request.StockQuantity,
                Tag = ParseTag(request.Tag),
                CategoryId = category.Id
            };

            await productRepository.AddAsync(product, ct);
            await productRepository.SaveChangesAsync(ct);
            product.Category = category;

            return ToDto(product);
        }

        public async Task<(ProductDto? Product, string? Error)> UpdateAsync(
            int id,
            UpdateProductRequest request,
            CancellationToken ct = default)
        {
            var product = await productRepository.GetByIdAsync(id, ct);
            if (product is null)
            {
                return (null, null);
            }

            var category = await categoryRepository.GetByIdAsync(request.CategoryId, ct);
            if (category is null)
            {
                return (null, $"Category with id '{request.CategoryId}' does not exist.");
            }

            product.Name = request.Name.Trim();
            product.Description = request.Description.Trim();
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;
            product.StockQuantity = request.StockQuantity;
            product.Tag = ParseTag(request.Tag);
            product.CategoryId = category.Id;
            product.Category = category;
            product.UpdatedAt = DateTime.UtcNow;

            await productRepository.UpdateAsync(product, ct);
            await productRepository.SaveChangesAsync(ct);

            return (ToDto(product), null);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var product = await productRepository.GetByIdAsync(id, ct);
            if (product is null)
            {
                return false;
            }

            await productRepository.DeleteAsync(product, ct);
            await productRepository.SaveChangesAsync(ct);

            return true;
        }

        private static ProductTag? ParseTag(string? tag) =>
            !string.IsNullOrWhiteSpace(tag) && Enum.TryParse<ProductTag>(tag, ignoreCase: true, out var parsed)
                ? parsed
                : null;

        private static ProductDto ToDto(Product product) => new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.ImageUrl,
            product.StockQuantity,
            product.Tag?.ToString(),
            product.CategoryId,
            product.Category.Name,
            product.Category.Slug);
    }
}
