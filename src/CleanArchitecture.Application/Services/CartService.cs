using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Dtos.Cart;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Services;

public sealed class CartService(
    ICartItemRepository cartItemRepository,
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICartService
{
    public async Task<CartDto> GetCartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var items = await cartItemRepository.GetByUserIdAsync(userId, cancellationToken);
        return MapCart(userId, items);
    }

    public async Task<CartDto> AddToCartAsync(Guid userId, AddToCartRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        var book = await bookRepository.GetByIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException("Book was not found.");

        var existingItem = await cartItemRepository.GetByUserAndBookAsync(userId, request.BookId, cancellationToken);
        if (existingItem is null)
        {
            await cartItemRepository.AddAsync(new CartItem
            {
                UserId = userId,
                BookId = book.Id,
                Book = book,
                Quantity = request.Quantity
            }, cancellationToken);
        }
        else
        {
            existingItem.Quantity += request.Quantity;
            existingItem.Book ??= book;
            cartItemRepository.Update(existingItem);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return await GetCartAsync(userId, cancellationToken);
    }

    public async Task<CartDto> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        var cartItem = await cartItemRepository.GetByIdAsync(cartItemId, cancellationToken);
        if (cartItem is null || cartItem.UserId != userId)
        {
            throw new NotFoundException("Cart item was not found.");
        }

        cartItem.Quantity = request.Quantity;
        cartItemRepository.Update(cartItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetCartAsync(userId, cancellationToken);
    }

    public async Task<CartDto> RemoveCartItemAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken = default)
    {
        var cartItem = await cartItemRepository.GetByIdAsync(cartItemId, cancellationToken);
        if (cartItem is null || cartItem.UserId != userId)
        {
            throw new NotFoundException("Cart item was not found.");
        }

        cartItemRepository.Remove(cartItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetCartAsync(userId, cancellationToken);
    }

    private static CartDto MapCart(Guid userId, IReadOnlyCollection<CartItem> items)
    {
        var cartItems = items.Select(item => new CartItemDto
        {
            Id = item.Id,
            BookId = item.BookId,
            BookTitle = item.Book?.Title ?? string.Empty,
            UnitPrice = item.Book?.Price ?? 0,
            Quantity = item.Quantity,
            TotalPrice = (item.Book?.Price ?? 0) * item.Quantity
        }).ToArray();

        return new CartDto
        {
            UserId = userId,
            Items = cartItems,
            TotalAmount = cartItems.Sum(item => item.TotalPrice)
        };
    }
}
