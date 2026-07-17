using Vector.Domain.Entities;

namespace Vector.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task AddAsync(Cart cart, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
