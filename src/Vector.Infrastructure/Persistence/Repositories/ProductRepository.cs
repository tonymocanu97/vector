using Microsoft.EntityFrameworkCore;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Repositories
{
    public class ProductRepository(VectorDbContext context) : IProductRepository
    {
        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default) =>
            await context.Products.Include(p => p.Category).AsNoTracking().ToListAsync(ct);

        public async Task<IReadOnlyList<Product>> GetByCategorySlugAsync(string categorySlug, CancellationToken ct = default) =>
            await context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Slug == categorySlug)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task AddAsync(Product product, CancellationToken ct = default) =>
            await context.Products.AddAsync(product, ct);

        public Task UpdateAsync(Product product, CancellationToken ct = default)
        {
            context.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product, CancellationToken ct = default)
        {
            context.Products.Remove(product);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken ct = default) =>
            await context.SaveChangesAsync(ct);
    }
}
