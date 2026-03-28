using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces.Persistence;

public interface IBookRepository
{
    Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    void Update(Book book);
    void Remove(Book book);
}
