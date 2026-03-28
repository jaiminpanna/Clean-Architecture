using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces.Persistence;

public interface ICartItemRepository
{
    Task<IReadOnlyCollection<CartItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetByUserAndBookAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);
    Task AddAsync(CartItem cartItem, CancellationToken cancellationToken = default);
    void Update(CartItem cartItem);
    void Remove(CartItem cartItem);
}
