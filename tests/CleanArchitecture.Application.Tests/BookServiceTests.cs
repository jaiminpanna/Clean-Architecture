using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Dtos.Books;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Application.Tests.Fakes;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests;

public sealed class BookServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldAddBook_WhenCategoryExists()
    {
        var categoryRepository = new FakeCategoryRepository();
        var bookRepository = new FakeBookRepository();
        var unitOfWork = new FakeUnitOfWork();
        var category = new Category { Name = "Fiction" };
        categoryRepository.Items.Add(category);

        var service = new BookService(bookRepository, categoryRepository, unitOfWork);

        var result = await service.CreateAsync(new CreateBookRequestDto
        {
            Title = "Clean Book",
            Author = "Author",
            Isbn = "123456789",
            Price = 250,
            StockQuantity = 10,
            CategoryId = category.Id
        });

        Assert.Equal("Clean Book", result.Title);
        Assert.Equal("Fiction", result.CategoryName);
        Assert.Single(bookRepository.Items);
        Assert.Equal(1, unitOfWork.SaveChangesCalls);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenBookDoesNotExist()
    {
        var service = new BookService(new FakeBookRepository(), new FakeCategoryRepository(), new FakeUnitOfWork());

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(Guid.NewGuid(), new UpdateBookRequestDto
        {
            Title = "Missing",
            Author = "Missing",
            Isbn = "000",
            Price = 20,
            StockQuantity = 1,
            CategoryId = Guid.NewGuid()
        }));
    }
}
