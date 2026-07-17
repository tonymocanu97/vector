using Vector.Application.DTOs;

namespace Vector.Application.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    }
}
