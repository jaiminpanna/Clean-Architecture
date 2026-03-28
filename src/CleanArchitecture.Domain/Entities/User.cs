using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public ICollection<CartItem> CartItems { get; set; } = [];
}
