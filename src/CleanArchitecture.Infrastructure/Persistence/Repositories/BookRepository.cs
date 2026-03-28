using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories;

public sealed class BookRepository(ApplicationDbContext dbContext) : IBookRepository
{
    public async Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Books
            .Include(book => book.Category)
            .AsNoTracking()
            .OrderBy(book => book.Title)
            .ToListAsync(cancellationToken);

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Books
            .Include(book => book.Category)
            .FirstOrDefaultAsync(book => book.Id == id, cancellationToken);

    public Task AddAsync(Book book, CancellationToken cancellationToken = default) =>
        dbContext.Books.AddAsync(book, cancellationToken).AsTask();

    public void Update(Book book) => dbContext.Books.Update(book);

    public void Remove(Book book) => dbContext.Books.Remove(book);
}
