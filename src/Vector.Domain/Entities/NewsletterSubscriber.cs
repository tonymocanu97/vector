namespace Vector.Domain.Entities
{
    public class NewsletterSubscriber
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Email { get; set; } = string.Empty;
    }
}
