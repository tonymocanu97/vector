using Microsoft.AspNetCore.Mvc;
using Vector.Application.DTOs;
using Vector.Application.Services;

namespace Vector.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(CancellationToken ct)
        {
            return Ok(await categoryService.GetAllAsync(ct));
        }
    }
}