using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Dtos.Cart;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Application.Tests.Fakes;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests;

public sealed class CartServiceTests
{
    [Fact]
    public async Task AddToCartAsync_ShouldMergeQuantity_ForExistingBook()
    {
        var userId = Guid.NewGuid();
        var book = new Book
        {
            Title = "Domain Driven Design",
            Author = "Eric Evans",
            Isbn = "9780321125217",
            Price = 500
        };

        var bookRepository = new FakeBookRepository();
        bookRepository.Items.Add(book);

        var cartRepository = new FakeCartItemRepository();
        cartRepository.Items.Add(new CartItem
        {
            UserId = userId,
            BookId = book.Id,
            Book = book,
            Quantity = 1
        });

        var service = new CartService(cartRepository, bookRepository, new FakeUnitOfWork());

        var result = await service.AddToCartAsync(userId, new AddToCartRequestDto
        {
            BookId = book.Id,
            Quantity = 2
        });

        Assert.Single(result.Items);
        Assert.Equal(3, result.Items.First().Quantity);
        Assert.Equal(1500, result.TotalAmount);
    }

    [Fact]
    public async Task UpdateCartItemAsync_ShouldThrow_WhenQuantityIsInvalid()
    {
        var service = new CartService(new FakeCartItemRepository(), new FakeBookRepository(), new FakeUnitOfWork());

        await Assert.ThrowsAsync<ValidationException>(() => service.UpdateCartItemAsync(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new UpdateCartItemRequestDto { Quantity = 0 }));
    }
}
