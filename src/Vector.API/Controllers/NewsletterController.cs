using Microsoft.AspNetCore.Mvc;
using Vector.Application.DTOs;
using Vector.Application.Services;

namespace Vector.API.Controllers
{
    [ApiController]
    [Route("api/newsletter")]
    public class NewsletterController(INewsletterService newsletterService) : ControllerBase
    {
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(SubscribeRequest request, CancellationToken ct)
        {
            await newsletterService.SubscribeAsync(request, ct);
            return Ok();
        }
    }
}
