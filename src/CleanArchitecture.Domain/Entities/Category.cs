namespace CleanArchitecture.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public ICollection<Book> Books { get; set; } = [];
}
