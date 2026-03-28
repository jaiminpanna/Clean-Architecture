using CleanArchitecture.Application.Dtos.Categories;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request, CancellationToken cancellationToken = default);
}
