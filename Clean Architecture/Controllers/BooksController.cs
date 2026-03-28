using CleanArchitecture.Application.Dtos.Books;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Route("api/books")]
public sealed class BooksController(IBookService bookService) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<BookDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await bookService.GetAllAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await bookService.GetByIdAsync(id, cancellationToken));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookRequestDto request, CancellationToken cancellationToken)
    {
        var book = await bookService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookDto>> Update(Guid id, UpdateBookRequestDto request, CancellationToken cancellationToken)
        => Ok(await bookService.UpdateAsync(id, request, cancellationToken));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await bookService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
