using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces.Security;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(User user);
}
