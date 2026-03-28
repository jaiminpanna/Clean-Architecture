using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests.Fakes;

internal sealed class FakeCartItemRepository : ICartItemRepository
{
    public List<CartItem> Items { get; } = [];

    public Task<IReadOnlyCollection<CartItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        Task.FromResult((IReadOnlyCollection<CartItem>)Items.Where(item => item.UserId == userId).ToArray());

    public Task<CartItem?> GetByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(item => item.Id == cartItemId));

    public Task<CartItem?> GetByUserAndBookAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(item => item.UserId == userId && item.BookId == bookId));

    public Task AddAsync(CartItem cartItem, CancellationToken cancellationToken = default)
    {
        Items.Add(cartItem);
        return Task.CompletedTask;
    }

    public void Update(CartItem cartItem)
    {
    }

    public void Remove(CartItem cartItem) => Items.Remove(cartItem);
}
