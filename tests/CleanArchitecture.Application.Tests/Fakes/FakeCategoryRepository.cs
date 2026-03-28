using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests.Fakes;

internal sealed class FakeCategoryRepository : ICategoryRepository
{
    public List<Category> Items { get; } = [];

    public Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult((IReadOnlyCollection<Category>)Items.OrderBy(category => category.Name).ToArray());

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(category => category.Id == id));

    public Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(category => string.Equals(category.Name, name, StringComparison.OrdinalIgnoreCase)));

    public Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        Items.Add(category);
        return Task.CompletedTask;
    }
}
