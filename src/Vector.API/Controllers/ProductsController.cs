using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vector.API.Controllers
{
    public record Product(int Id, string Name, string Category, decimal Price);

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly Product[] Products =
        [
            new(1, "Pro Kit 2026 Jacket", "Pro Kit", 129.99m),
            new(2, "Pro Kit 2026 Joggers", "Pro Kit", 89.99m),
            new(3, "Base Collection Tee", "Apparel", 39.99m),
            new(4, "Wireless Mouse", "Hardware", 79.99m),
            new(5, "Team Cap", "Accessories", 24.99m)
        ];

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(Products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            return product is null ? NotFound() : Ok(product);
        }

        [Authorize]
        [HttpGet("mine")]
        public ActionResult<IEnumerable<Product>> GetMine()
        {
            return Ok(Products.Where(p => p.Category == "Pro Kit"));
        }
    }
}
