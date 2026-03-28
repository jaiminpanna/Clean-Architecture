using CleanArchitecture.Application.Dtos.Auth;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("admin/signup")]
    public async Task<ActionResult<AuthResponseDto>> AdminSignup(RegisterRequestDto request, CancellationToken cancellationToken)
        => Ok(await authService.RegisterAdminAsync(request, cancellationToken));

    [AllowAnonymous]
    [HttpPost("admin/login")]
    public async Task<ActionResult<AuthResponseDto>> AdminLogin(LoginRequestDto request, CancellationToken cancellationToken)
        => Ok(await authService.LoginAdminAsync(request, cancellationToken));

    [AllowAnonymous]
    [HttpPost("user/signup")]
    public async Task<ActionResult<AuthResponseDto>> UserSignup(RegisterRequestDto request, CancellationToken cancellationToken)
        => Ok(await authService.RegisterUserAsync(request, cancellationToken));

    [AllowAnonymous]
    [HttpPost("user/login")]
    public async Task<ActionResult<AuthResponseDto>> UserLogin(LoginRequestDto request, CancellationToken cancellationToken)
        => Ok(await authService.LoginUserAsync(request, cancellationToken));

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me(CancellationToken cancellationToken)
        => Ok(await authService.GetByIdAsync(GetUserId(), cancellationToken));
}
