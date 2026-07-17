using Microsoft.EntityFrameworkCore;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Repositories
{
    public class UserRepository(VectorDbContext context) : IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
            await context.Users.AnyAsync(u => u.Email == email, ct);

        public async Task AddAsync(User user, CancellationToken ct = default) =>
            await context.Users.AddAsync(user, ct);

        public async Task SaveChangesAsync(CancellationToken ct = default) =>
            await context.SaveChangesAsync(ct);
    }
}
