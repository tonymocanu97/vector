using Microsoft.EntityFrameworkCore;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Infrastructure.Persistence.Repositories
{
    public class NewsletterSubscriberRepository(VectorDbContext context) : INewsletterSubscriberRepository
    {
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
            await context.NewsletterSubscribers.AnyAsync(n => n.Email == email, ct);

        public async Task AddAsync(NewsletterSubscriber subscriber, CancellationToken ct = default) =>
            await context.NewsletterSubscribers.AddAsync(subscriber, ct);

        public async Task SaveChangesAsync(CancellationToken ct = default) =>
            await context.SaveChangesAsync(ct);
    }
}
