using CleanArchitecture.Application.Dtos.Categories;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Route("api/categories")]
public sealed class CategoriesController(ICategoryService categoryService) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CategoryDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await categoryService.GetAllAsync(cancellationToken));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryRequestDto request, CancellationToken cancellationToken)
        => Ok(await categoryService.CreateAsync(request, cancellationToken));
}
