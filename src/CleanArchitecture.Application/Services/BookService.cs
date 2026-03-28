using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Dtos.Books;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Services;

public sealed class BookService(
    IBookRepository bookRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IBookService
{
    public async Task<IReadOnlyCollection<BookDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var books = await bookRepository.GetAllAsync(cancellationToken);
        return books.Select(MapBook).ToArray();
    }

    public async Task<BookDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Book was not found.");

        return MapBook(book);
    }

    public async Task<BookDto> CreateAsync(CreateBookRequestDto request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException("Category was not found.");

        var book = new Book
        {
            Title = request.Title.Trim(),
            Author = request.Author.Trim(),
            Isbn = request.Isbn.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = category.Id,
            Category = category
        };

        await bookRepository.AddAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapBook(book);
    }

    public async Task<BookDto> UpdateAsync(Guid id, UpdateBookRequestDto request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var book = await bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Book was not found.");

        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException("Category was not found.");

        book.Title = request.Title.Trim();
        book.Author = request.Author.Trim();
        book.Isbn = request.Isbn.Trim();
        book.Description = request.Description?.Trim();
        book.Price = request.Price;
        book.StockQuantity = request.StockQuantity;
        book.CategoryId = category.Id;
        book.Category = category;
        book.UpdatedOnUtc = DateTime.UtcNow;

        bookRepository.Update(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapBook(book);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Book was not found.");

        bookRepository.Remove(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateRequest(CreateBookRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Author) ||
            string.IsNullOrWhiteSpace(request.Isbn))
        {
            throw new ValidationException("Title, author and ISBN are required.");
        }

        if (request.Price < 0)
        {
            throw new ValidationException("Price cannot be negative.");
        }

        if (request.StockQuantity < 0)
        {
            throw new ValidationException("Stock quantity cannot be negative.");
        }
    }

    private static BookDto MapBook(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Isbn = book.Isbn,
        Description = book.Description,
        Price = book.Price,
        StockQuantity = book.StockQuantity,
        CategoryId = book.CategoryId,
        CategoryName = book.Category?.Name ?? string.Empty
    };
}
