using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(ApplicationDbContext dbContext) : ICategoryRepository
{
    public async Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Categories.FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

    public Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        dbContext.Categories.FirstOrDefaultAsync(
            category => category.Name.ToLower() == name.Trim().ToLower(),
            cancellationToken);

    public Task AddAsync(Category category, CancellationToken cancellationToken = default) =>
        dbContext.Categories.AddAsync(category, cancellationToken).AsTask();
}
