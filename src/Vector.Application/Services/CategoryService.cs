using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Application.Services
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken ct = default)
        {
            var categories = await categoryRepository.GetAllAsync(ct);
            return categories.Select(ToDto).ToList();
        }

        private static CategoryDto ToDto(Category category) =>
            new(category.Id, category.Name, category.Slug, category.Description);
    }
}
