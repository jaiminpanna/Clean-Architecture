using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests.Fakes;

internal sealed class FakeUserRepository : IUserRepository
{
    public List<User> Items { get; } = [];

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(user => user.Email == email.Trim().ToLowerInvariant()));

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.FirstOrDefault(user => user.Id == id));

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        Items.Add(user);
        return Task.CompletedTask;
    }
}
