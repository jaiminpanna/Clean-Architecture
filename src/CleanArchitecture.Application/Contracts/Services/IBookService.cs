using CleanArchitecture.Application.Dtos.Books;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IBookService
{
    Task<IReadOnlyCollection<BookDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BookDto> CreateAsync(CreateBookRequestDto request, CancellationToken cancellationToken = default);
    Task<BookDto> UpdateAsync(Guid id, UpdateBookRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
