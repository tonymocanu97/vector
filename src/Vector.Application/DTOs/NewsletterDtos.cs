using System.ComponentModel.DataAnnotations;

namespace Vector.Application.DTOs
{
    public record SubscribeRequest([Required, EmailAddress] string Email);
}
