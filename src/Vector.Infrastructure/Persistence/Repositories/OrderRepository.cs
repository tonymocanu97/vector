using Microsoft.EntityFrameworkCore;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Repositories
{
    public class OrderRepository(VectorDbContext context) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id, ct);

        public async Task<IReadOnlyList<Order>> GetByUserIdAsync(int userId, CancellationToken ct = default) =>
            await context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task AddAsync(Order order, CancellationToken ct = default) =>
            await context.Orders.AddAsync(order, ct);

        public async Task SaveChangesAsync(CancellationToken ct = default) =>
            await context.SaveChangesAsync(ct);
    }
}
