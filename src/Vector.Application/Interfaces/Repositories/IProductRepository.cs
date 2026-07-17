using Vector.Domain.Entities;

namespace Vector.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<Product>> GetByCategorySlugAsync(string categorySlug, CancellationToken ct = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(Product product, CancellationToken ct = default);
        Task UpdateAsync(Product product, CancellationToken ct = default);
        Task DeleteAsync(Product product, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
