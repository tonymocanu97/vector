using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;
using Vector.Domain.Enums;

namespace Vector.Application.Services
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }

    public interface IJwtTokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(User user);
    }

    public class AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService) : IAuthService
    {
        public async Task<(AuthResponse? Response, string? Error)> RegisterAsync(
            RegisterRequest request,
            CancellationToken ct = default)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            if (await userRepository.ExistsByEmailAsync(normalizedEmail, ct))
            {
                return (null, $"An account with email '{normalizedEmail}' already exists.");
            }

            var user = new User
            {
                Email = normalizedEmail,
                PasswordHash = passwordHasher.Hash(request.Password),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Role = UserRole.Customer
            };

            await userRepository.AddAsync(user, ct);
            await userRepository.SaveChangesAsync(ct);

            return (BuildAuthResponse(user), null);
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var user = await userRepository.GetByEmailAsync(normalizedEmail, ct);

            if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            return BuildAuthResponse(user);
        }

        private AuthResponse BuildAuthResponse(User user)
        {
            var (token, expiresAt) = jwtTokenService.GenerateToken(user);
            var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role.ToString());
            return new AuthResponse(token, expiresAt, userDto);
        }
    }
}
