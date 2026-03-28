using CleanArchitecture.Application.Dtos.Auth;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAdminAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginAdminAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginUserAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
