using CleanArchitecture.Application.Dtos.Cart;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartDto> GetCartAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CartDto> AddToCartAsync(Guid userId, AddToCartRequestDto request, CancellationToken cancellationToken = default);
    Task<CartDto> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemRequestDto request, CancellationToken cancellationToken = default);
    Task<CartDto> RemoveCartItemAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken = default);
}
