namespace CleanArchitecture.Application.Dtos.Cart;

public sealed class CartDto
{
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public IReadOnlyCollection<CartItemDto> Items { get; set; } = [];
}
