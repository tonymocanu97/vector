using Vector.Application.DTOs;

namespace Vector.Application.Services
{
    public interface INewsletterService
    {
        // Idempotent - re-subscribing an existing email is a no-op, not an error.
        Task SubscribeAsync(SubscribeRequest request, CancellationToken ct = default);
    }
}
