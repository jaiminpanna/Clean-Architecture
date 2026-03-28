using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories;

public sealed class CartItemRepository(ApplicationDbContext dbContext) : ICartItemRepository
{
    public async Task<IReadOnlyCollection<CartItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await dbContext.CartItems
            .Include(cartItem => cartItem.Book)
            .AsNoTracking()
            .Where(cartItem => cartItem.UserId == userId)
            .OrderByDescending(cartItem => cartItem.CreatedOnUtc)
            .ToListAsync(cancellationToken);

    public Task<CartItem?> GetByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default) =>
        dbContext.CartItems
            .Include(cartItem => cartItem.Book)
            .FirstOrDefaultAsync(cartItem => cartItem.Id == cartItemId, cancellationToken);

    public Task<CartItem?> GetByUserAndBookAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default) =>
        dbContext.CartItems
            .Include(cartItem => cartItem.Book)
            .FirstOrDefaultAsync(cartItem => cartItem.UserId == userId && cartItem.BookId == bookId, cancellationToken);

    public Task AddAsync(CartItem cartItem, CancellationToken cancellationToken = default) =>
        dbContext.CartItems.AddAsync(cartItem, cancellationToken).AsTask();

    public void Update(CartItem cartItem) => dbContext.CartItems.Update(cartItem);

    public void Remove(CartItem cartItem) => dbContext.CartItems.Remove(cartItem);
}
