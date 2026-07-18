using Vector.Domain.Entities;

namespace Vector.Application.Interfaces.Repositories
{
    public interface INewsletterSubscriberRepository
    {
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task AddAsync(NewsletterSubscriber subscriber, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
