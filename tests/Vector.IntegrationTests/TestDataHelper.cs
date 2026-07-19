using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Vector.Application.DTOs;
using Vector.Application.Services;
using Vector.Domain.Entities;
using Vector.Domain.Enums;
using Vector.Infrastructure.Persistence;

namespace Vector.IntegrationTests
{
    internal static class TestDataHelper
    {
        public const string Password = "Password123!";

        public static async Task<string> RegisterAndLoginAsync(HttpClient client)
        {
            var email = $"{Guid.NewGuid():N}@example.com";
            var request = new RegisterRequest(email, Password, "Jane", "Doe");

            var response = await client.PostAsJsonAsync("/api/auth/register", request);
            response.EnsureSuccessStatusCode();

            var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return auth!.Token;
        }

        public static async Task<string> CreateAdminAndLoginAsync(HttpClient client, ApiWebApplicationFactory factory)
        {
            var email = $"{Guid.NewGuid():N}@example.com";

            using (var scope = factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<VectorDbContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

                dbContext.Users.Add(new User
                {
                    Email = email,
                    PasswordHash = passwordHasher.Hash(Password),
                    FirstName = "Admin",
                    LastName = "User",
                    Role = UserRole.Admin
                });
                await dbContext.SaveChangesAsync();
            }

            var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest(email, Password));
            response.EnsureSuccessStatusCode();

            var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return auth!.Token;
        }
    }
}
