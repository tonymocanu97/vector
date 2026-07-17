using Vector.Domain.Entities;

namespace Vector.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Order>> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task AddAsync(Order order, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
