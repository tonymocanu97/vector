using Vector.Application.DTOs;

namespace Vector.Application.Services
{
    public interface IAuthService
    {
        // Error is set (email already registered) -> 409; otherwise Response is populated.
        Task<(AuthResponse? Response, string? Error)> RegisterAsync(RegisterRequest request, CancellationToken ct = default);

        // Null on invalid credentials -> 401.
        Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default);
    }
}
