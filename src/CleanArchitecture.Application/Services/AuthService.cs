using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Dtos.Auth;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Application.Interfaces.Security;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public Task<AuthResponseDto> RegisterAdminAsync(RegisterRequestDto request, CancellationToken cancellationToken = default) =>
        RegisterAsync(request, UserRole.Admin, cancellationToken);

    public Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto request, CancellationToken cancellationToken = default) =>
        RegisterAsync(request, UserRole.User, cancellationToken);

    public Task<AuthResponseDto> LoginAdminAsync(LoginRequestDto request, CancellationToken cancellationToken = default) =>
        LoginAsync(request, UserRole.Admin, cancellationToken);

    public Task<AuthResponseDto> LoginUserAsync(LoginRequestDto request, CancellationToken cancellationToken = default) =>
        LoginAsync(request, UserRole.User, cancellationToken);

    public async Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("User was not found.");

        return MapUser(user);
    }

    private async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, UserRole role, CancellationToken cancellationToken)
    {
        ValidateRegistration(request);

        var existingUser = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            throw new ConflictException("A user with this email already exists.");
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = role
        };

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CreateAuthResponse(user);
    }

    private async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, UserRole role, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ValidationException("Email and password are required.");
        }

        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || user.Role != role || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        return CreateAuthResponse(user);
    }

    private AuthResponseDto CreateAuthResponse(User user)
    {
        var (token, expiresAtUtc) = jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc,
            User = MapUser(user)
        };
    }

    private static UserDto MapUser(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Role = user.Role.ToString()
    };

    private static void ValidateRegistration(RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ValidationException("First name, last name, email and password are required.");
        }

        if (request.Password.Length < 6)
        {
            throw new ValidationException("Password must be at least 6 characters long.");
        }
    }
}
