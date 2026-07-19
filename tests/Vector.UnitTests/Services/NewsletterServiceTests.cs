using Vector.Application.DTOs;
using Vector.Application.Interfaces.Repositories;
using Vector.Application.Services;
using Vector.Domain.Entities;

namespace Vector.UnitTests.Services
{
    public class NewsletterServiceTests
    {
        private readonly Mock<INewsletterSubscriberRepository> _repository = new();
        private readonly NewsletterService _sut;

        public NewsletterServiceTests()
        {
            _sut = new NewsletterService(_repository.Object);
        }

        [Fact]
        public async Task SubscribeAsync_AlreadySubscribed_DoesNotAddAgain()
        {
            _repository.Setup(r => r.ExistsByEmailAsync("existing@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _sut.SubscribeAsync(new SubscribeRequest("existing@example.com"));

            _repository.Verify(r => r.AddAsync(It.IsAny<NewsletterSubscriber>(), It.IsAny<CancellationToken>()), Times.Never);
            _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task SubscribeAsync_NewEmail_NormalizesAndPersists()
        {
            _repository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await _sut.SubscribeAsync(new SubscribeRequest(" New.Subscriber@Example.com "));

            _repository.Verify(
                r => r.AddAsync(
                    It.Is<NewsletterSubscriber>(s => s.Email == "new.subscriber@example.com"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
