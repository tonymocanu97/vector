using Microsoft.AspNetCore.Mvc;
using Vector.Application.DTOs;
using Vector.Application.Services;

namespace Vector.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
        {
            var (response, error) = await authService.RegisterAsync(request, ct);
            return error is not null ? Conflict(error) : Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
        {
            var response = await authService.LoginAsync(request, ct);
            return response is null ? Unauthorized("Invalid email or password.") : Ok(response);
        }
    }
}
