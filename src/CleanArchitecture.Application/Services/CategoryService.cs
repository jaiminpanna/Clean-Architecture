using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Dtos.Categories;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Services;

public sealed class CategoryService(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : ICategoryService
{
    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(MapCategory).ToArray();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException("Category name is required.");
        }

        var existingCategory = await categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingCategory is not null)
        {
            throw new ConflictException("A category with this name already exists.");
        }

        var category = new Category
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim()
        };

        await categoryRepository.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapCategory(category);
    }

    private static CategoryDto MapCategory(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
    };
}
