using CleanArchitecture.Application.Interfaces.Security;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests.Fakes;

internal sealed class FakeJwtTokenGenerator : IJwtTokenGenerator
{
    public (string Token, DateTime ExpiresAtUtc) GenerateToken(User user) =>
        ($"token-for-{user.Email}", DateTime.UtcNow.AddHours(1));
}
