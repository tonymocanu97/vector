using System.Net;
using System.Net.Http.Json;
using Vector.Application.DTOs;

namespace Vector.IntegrationTests
{
    [Collection(ApiTestCollection.Name)]
    public class AuthEndpointsTests(ApiWebApplicationFactory factory)
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Register_NewUser_ReturnsOkWithToken()
        {
            var email = $"{Guid.NewGuid():N}@example.com";
            var request = new RegisterRequest(email, TestDataHelper.Password, "Jane", "Doe");

            var response = await _client.PostAsJsonAsync("/api/auth/register", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
            body!.Token.Should().NotBeNullOrEmpty();
            body.User.Email.Should().Be(email);
            body.User.Role.Should().Be("Customer");
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsConflict()
        {
            var email = $"{Guid.NewGuid():N}@example.com";
            var request = new RegisterRequest(email, TestDataHelper.Password, "Jane", "Doe");
            await _client.PostAsJsonAsync("/api/auth/register", request);

            var response = await _client.PostAsJsonAsync("/api/auth/register", request);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            var email = $"{Guid.NewGuid():N}@example.com";
            await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequest(email, TestDataHelper.Password, "Jane", "Doe"));

            var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest(email, TestDataHelper.Password));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
            body!.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsUnauthorized()
        {
            var email = $"{Guid.NewGuid():N}@example.com";
            await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequest(email, TestDataHelper.Password, "Jane", "Doe"));

            var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest(email, "WrongPassword!"));

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_UnknownEmail_ReturnsUnauthorized()
        {
            var response = await _client.PostAsJsonAsync(
                "/api/auth/login",
                new LoginRequest($"{Guid.NewGuid():N}@example.com", TestDataHelper.Password));

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
