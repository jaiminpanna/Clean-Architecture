using CleanArchitecture.Application.Dtos.Cart;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Authorize(Roles = "User")]
[Route("api/cart")]
public sealed class CartController(ICartService cartService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<CartDto>> Get(CancellationToken cancellationToken)
        => Ok(await cartService.GetCartAsync(GetUserId(), cancellationToken));

    [HttpPost("items")]
    public async Task<ActionResult<CartDto>> Add(AddToCartRequestDto request, CancellationToken cancellationToken)
        => Ok(await cartService.AddToCartAsync(GetUserId(), request, cancellationToken));

    [HttpPut("items/{cartItemId:guid}")]
    public async Task<ActionResult<CartDto>> Update(Guid cartItemId, UpdateCartItemRequestDto request, CancellationToken cancellationToken)
        => Ok(await cartService.UpdateCartItemAsync(GetUserId(), cartItemId, request, cancellationToken));

    [HttpDelete("items/{cartItemId:guid}")]
    public async Task<ActionResult<CartDto>> Remove(Guid cartItemId, CancellationToken cancellationToken)
        => Ok(await cartService.RemoveCartItemAsync(GetUserId(), cartItemId, cancellationToken));
}
