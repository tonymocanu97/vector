using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vector.Application.DTOs;
using Vector.Application.Services;

namespace Vector.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("checkout")]
        public async Task<ActionResult<OrderDto>> Checkout(CheckoutRequest request, CancellationToken ct)
        {
            var (order, error) = await orderService.CheckoutAsync(CurrentUserId, request, ct);
            if (error is not null)
            {
                return BadRequest(error);
            }

            return CreatedAtAction(nameof(GetById), new { id = order!.Id }, order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll(CancellationToken ct)
        {
            return Ok(await orderService.GetOrdersAsync(CurrentUserId, ct));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken ct)
        {
            var order = await orderService.GetOrderByIdAsync(CurrentUserId, id, ct);
            return order is null ? NotFound() : Ok(order);
        }
    }
}
