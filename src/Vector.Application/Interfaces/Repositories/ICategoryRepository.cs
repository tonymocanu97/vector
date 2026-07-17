using Vector.Domain.Entities;

namespace Vector.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default);
        Task<Category?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<Category?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}
