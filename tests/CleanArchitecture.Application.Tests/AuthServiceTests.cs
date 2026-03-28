using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Dtos.Auth;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Application.Tests.Fakes;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Tests;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task RegisterUserAsync_ShouldCreateUserAndReturnToken()
    {
        var userRepository = new FakeUserRepository();
        var unitOfWork = new FakeUnitOfWork();
        var service = new AuthService(userRepository, unitOfWork, new FakePasswordHasher(), new FakeJwtTokenGenerator());

        var result = await service.RegisterUserAsync(new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Reader",
            Email = "john@example.com",
            Password = "password123"
        });

        Assert.Equal("john@example.com", result.User.Email);
        Assert.Equal("User", result.User.Role);
        Assert.StartsWith("token-for-", result.Token);
        Assert.Single(userRepository.Items);
        Assert.Equal(UserRole.User, userRepository.Items[0].Role);
        Assert.Equal(1, unitOfWork.SaveChangesCalls);
    }

    [Fact]
    public async Task LoginAdminAsync_ShouldThrow_WhenRoleDoesNotMatch()
    {
        var userRepository = new FakeUserRepository();
        userRepository.Items.Add(new User
        {
            FirstName = "Jane",
            LastName = "User",
            Email = "jane@example.com",
            PasswordHash = "HASH::password123",
            Role = UserRole.User
        });

        var service = new AuthService(userRepository, new FakeUnitOfWork(), new FakePasswordHasher(), new FakeJwtTokenGenerator());

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.LoginAdminAsync(new LoginRequestDto
        {
            Email = "jane@example.com",
            Password = "password123"
        }));
    }
}
