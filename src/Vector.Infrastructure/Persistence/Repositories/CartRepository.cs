using Microsoft.EntityFrameworkCore;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Repositories
{
    public class CartRepository(VectorDbContext context) : ICartRepository
    {
        public async Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default) =>
            await context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        public async Task AddAsync(Cart cart, CancellationToken ct = default) =>
            await context.Carts.AddAsync(cart, ct);

        public async Task SaveChangesAsync(CancellationToken ct = default) =>
            await context.SaveChangesAsync(ct);
    }
}
