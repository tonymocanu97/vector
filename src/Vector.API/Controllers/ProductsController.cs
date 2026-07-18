using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vector.Application.DTOs;
using Vector.Application.Services;

namespace Vector.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(
            [FromQuery] string? category,
            CancellationToken ct)
        {
            var products = string.IsNullOrWhiteSpace(category)
                ? await productService.GetAllAsync(ct)
                : await productService.GetByCategorySlugAsync(category, ct);

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken ct)
        {
            var product = await productService.GetByIdAsync(id, ct);
            return product is null ? NotFound($"Product with id '{id}' was not found.") : Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(CreateProductRequest request, CancellationToken ct)
        {
            var product = await productService.CreateAsync(request, ct);
            if (product is null)
            {
                return BadRequest($"Category with id '{request.CategoryId}' does not exist.");
            }

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductRequest request, CancellationToken ct)
        {
            var (product, error) = await productService.UpdateAsync(id, request, ct);
            if (error is not null)
            {
                return BadRequest(error);
            }

            return product is null ? NotFound($"Product with id '{id}' was not found.") : Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await productService.DeleteAsync(id, ct);
            return deleted ? NoContent() : NotFound($"Product with id '{id}' was not found.");
        }
    }
}
