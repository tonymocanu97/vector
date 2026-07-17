using System.ComponentModel.DataAnnotations;

namespace Vector.Application.DTOs
{
    public record RegisterRequest(
        [Required, EmailAddress] string Email,
        [Required, MinLength(8)] string Password,
        [Required] string FirstName,
        [Required] string LastName);

    public record LoginRequest([Required, EmailAddress] string Email, [Required] string Password);

    public record UserDto(int Id, string Email, string FirstName, string LastName, string Role);

    public record AuthResponse(string Token, DateTime ExpiresAt, UserDto User);
}
