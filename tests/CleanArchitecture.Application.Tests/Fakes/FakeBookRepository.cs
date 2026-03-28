using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests.Fakes;

internal sealed class FakeBookRepository : IBookRepository
{
    public List<Book> Items { get; } = [];

    public Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult((IReadOnlyCollection<Book>)Items.OrderBy(book => book.Title).ToArray());

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(book => book.Id == id));

    public Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        Items.Add(book);
        return Task.CompletedTask;
    }

    public void Update(Book book)
    {
    }

    public void Remove(Book book) => Items.Remove(book);
}
