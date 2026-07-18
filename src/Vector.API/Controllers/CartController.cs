using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vector.Application.DTOs;
using Vector.Application.Services;

namespace Vector.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/cart")]
    public class CartController(ICartService cartService) : ControllerBase
    {
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart(CancellationToken ct)
        {
            return Ok(await cartService.GetCartAsync(CurrentUserId, ct));
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddItem(AddCartItemRequest request, CancellationToken ct)
        {
            var (cart, error) = await cartService.AddItemAsync(CurrentUserId, request, ct);
            return error is not null ? BadRequest(error) : Ok(cart);
        }

        [HttpPut("items/{productId:int}")]
        public async Task<ActionResult<CartDto>> UpdateItem(
            int productId,
            UpdateCartItemRequest request,
            CancellationToken ct)
        {
            var (cart, error) = await cartService.UpdateItemAsync(CurrentUserId, productId, request, ct);
            if (error is not null)
            {
                return BadRequest(error);
            }

            return cart is null ? NotFound() : Ok(cart);
        }

        [HttpDelete("items/{productId:int}")]
        public async Task<ActionResult<CartDto>> RemoveItem(int productId, CancellationToken ct)
        {
            var cart = await cartService.RemoveItemAsync(CurrentUserId, productId, ct);
            return cart is null ? NotFound() : Ok(cart);
        }
    }
}
