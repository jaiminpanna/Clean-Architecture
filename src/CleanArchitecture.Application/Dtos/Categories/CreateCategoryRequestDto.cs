namespace CleanArchitecture.Application.Dtos.Categories;

public sealed class CreateCategoryRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
