using Vector.Application.DTOs;

namespace Vector.Application.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<ProductDto>> GetByCategorySlugAsync(string categorySlug, CancellationToken ct = default);

        // Null -> 404.
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);

        // Null -> 400 (CategoryId doesn't exist).
        Task<ProductDto?> CreateAsync(CreateProductRequest request, CancellationToken ct = default);

        // (null, null) -> 404 (product not found). (null, error) -> 400 (CategoryId doesn't exist).
        Task<(ProductDto? Product, string? Error)> UpdateAsync(
            int id,
            UpdateProductRequest request,
            CancellationToken ct = default);

        // False -> 404.
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
