using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(category => category.Name).IsUnique();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");
            entity.HasKey(book => book.Id);
            entity.Property(book => book.Title).HasMaxLength(200).IsRequired();
            entity.Property(book => book.Author).HasMaxLength(150).IsRequired();
            entity.Property(book => book.Isbn).HasMaxLength(50).IsRequired();
            entity.Property(book => book.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(book => book.Category)
                .WithMany(category => category.Books)
                .HasForeignKey(book => book.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(user => user.LastName).HasMaxLength(100).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(200).IsRequired();
            entity.Property(user => user.PasswordHash).IsRequired();
            entity.Property(user => user.Role)
                .HasConversion(
                    role => role.ToString(),
                    value => Enum.Parse<UserRole>(value));
            entity.HasIndex(user => user.Email).IsUnique();
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItems");
            entity.HasKey(cartItem => cartItem.Id);
            entity.HasOne(cartItem => cartItem.User)
                .WithMany(user => user.CartItems)
                .HasForeignKey(cartItem => cartItem.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(cartItem => cartItem.Book)
                .WithMany(book => book.CartItems)
                .HasForeignKey(cartItem => cartItem.BookId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(cartItem => new { cartItem.UserId, cartItem.BookId }).IsUnique();
        });
    }
}
