using Microsoft.EntityFrameworkCore;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository(VectorDbContext context) : ICategoryRepository
    {
        public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default) =>
            await context.Categories.AsNoTracking().ToListAsync(ct);

        public async Task<Category?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
            await context.Categories.FirstOrDefaultAsync(c => c.Slug == slug, ct);

        public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
    }
}
