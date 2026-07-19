using Microsoft.Extensions.Logging.Abstractions;
using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Application.Services;
using Vector.Domain.Entities;
using Vector.Domain.Enums;

namespace Vector.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<IJwtTokenService> _jwtTokenService = new();
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            _sut = new AuthService(
                _userRepository.Object,
                _passwordHasher.Object,
                _jwtTokenService.Object,
                NullLogger<AuthService>.Instance);
        }

        [Fact]
        public async Task RegisterAsync_EmailAlreadyRegistered_ReturnsError()
        {
            _userRepository.Setup(r => r.ExistsByEmailAsync("taken@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var request = new RegisterRequest("taken@example.com", "password123", "Jane", "Doe");

            var (response, error) = await _sut.RegisterAsync(request);

            response.Should().BeNull();
            error.Should().Contain("taken@example.com");
            _userRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_NewEmail_HashesPasswordAndPersistsUser()
        {
            _userRepository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _passwordHasher.Setup(h => h.Hash("password123")).Returns("hashed-password");
            _jwtTokenService.Setup(j => j.GenerateToken(It.IsAny<User>()))
                .Returns(("jwt-token", DateTime.UtcNow.AddHours(1)));

            var request = new RegisterRequest("New.User@Example.com", "password123", "Jane", "Doe");

            var (response, error) = await _sut.RegisterAsync(request);

            error.Should().BeNull();
            response.Should().NotBeNull();
            response!.Token.Should().Be("jwt-token");
            response.User.Email.Should().Be("new.user@example.com");
            response.User.Role.Should().Be(nameof(UserRole.Customer));

            _userRepository.Verify(
                r => r.AddAsync(
                    It.Is<User>(u => u.Email == "new.user@example.com" && u.PasswordHash == "hashed-password"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            _userRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_UnknownEmail_ReturnsNull()
        {
            _userRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var result = await _sut.LoginAsync(new LoginRequest("nobody@example.com", "password123"));

            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            var user = new User { Id = 1, Email = "jane@example.com", PasswordHash = "hashed-password" };
            _userRepository.Setup(r => r.GetByEmailAsync("jane@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _passwordHasher.Setup(h => h.Verify("wrong-password", "hashed-password")).Returns(false);

            var result = await _sut.LoginAsync(new LoginRequest("jane@example.com", "wrong-password"));

            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
        {
            var user = new User
            {
                Id = 1,
                Email = "jane@example.com",
                PasswordHash = "hashed-password",
                FirstName = "Jane",
                LastName = "Doe",
                Role = UserRole.Customer
            };
            _userRepository.Setup(r => r.GetByEmailAsync("jane@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _passwordHasher.Setup(h => h.Verify("password123", "hashed-password")).Returns(true);
            _jwtTokenService.Setup(j => j.GenerateToken(user)).Returns(("jwt-token", DateTime.UtcNow.AddHours(1)));

            var result = await _sut.LoginAsync(new LoginRequest("jane@example.com", "password123"));

            result.Should().NotBeNull();
            result!.Token.Should().Be("jwt-token");
            result.User.Id.Should().Be(1);
        }
    }
}
