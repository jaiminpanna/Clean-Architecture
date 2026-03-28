namespace CleanArchitecture.Domain.Entities;

public sealed class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
}
