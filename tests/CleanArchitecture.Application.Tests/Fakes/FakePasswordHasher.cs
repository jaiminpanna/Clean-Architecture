using CleanArchitecture.Application.Interfaces.Security;

namespace CleanArchitecture.Application.Tests.Fakes;

internal sealed class FakePasswordHasher : IPasswordHasher
{
    public string Hash(string password) => $"HASH::{password}";

    public bool Verify(string password, string hashedPassword) => hashedPassword == Hash(password);
}
