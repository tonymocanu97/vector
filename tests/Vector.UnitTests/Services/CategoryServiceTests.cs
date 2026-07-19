using Vector.Application.Interfaces.Repositories;
using Vector.Application.Services;
using Vector.Domain.Entities;

namespace Vector.UnitTests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepository = new();
        private readonly CategoryService _sut;

        public CategoryServiceTests()
        {
            _sut = new CategoryService(_categoryRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedCategories()
        {
            var category = new Category { Id = 1, Name = "Apparel", Slug = "apparel", Description = "Wearables" };
            _categoryRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IReadOnlyList<Category>)[category]);

            var result = await _sut.GetAllAsync();

            result.Should().ContainSingle(c => c.Id == 1 && c.Slug == "apparel" && c.Description == "Wearables");
        }

        [Fact]
        public async Task GetAllAsync_NoCategories_ReturnsEmptyList()
        {
            _categoryRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IReadOnlyList<Category>)[]);

            var result = await _sut.GetAllAsync();

            result.Should().BeEmpty();
        }
    }
}
