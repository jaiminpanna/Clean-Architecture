namespace CleanArchitecture.Application.Dtos.Cart;

public sealed class AddToCartRequestDto
{
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
}
