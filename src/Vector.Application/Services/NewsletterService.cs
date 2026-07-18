using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Domain.Entities;

namespace Vector.Application.Services
{
    public class NewsletterService(INewsletterSubscriberRepository repository) : INewsletterService
    {
        public async Task SubscribeAsync(SubscribeRequest request, CancellationToken ct = default)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            if (await repository.ExistsByEmailAsync(normalizedEmail, ct))
            {
                return;
            }

            await repository.AddAsync(new NewsletterSubscriber { Email = normalizedEmail }, ct);
            await repository.SaveChangesAsync(ct);
        }
    }
}
