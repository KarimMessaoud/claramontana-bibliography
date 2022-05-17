using ClaramontanaBibliography.Data.Entities;
using ClaramontanaBibliography.Service;
using ClaramontanaBibliography.WebApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryItemService _libraryItemService;
        public BooksController(ILibraryItemService libraryItemService)
        {
            _libraryItemService = libraryItemService;
        }

        [HttpGet]
        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = (await _libraryItemService.GetAllBooksAsync()).Select(x => new BookDto
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                Year = x.Year,
                NumberOfPages = x.NumberOfPages
            });
            return books;
        }

        [ActionName("GetBookAsync")] //This attribute is needed for the proper link generation in CreateBook method,
                                     //because by default the Async suffix is trimmed
        [HttpGet("{bookId:guid}")]
        public async Task<ActionResult<BookDto>> GetBookAsync(Guid bookId)
        {
            var book = await _libraryItemService.GetBookAsync(bookId);

            if(book == null)
            {
                return NotFound();
            }

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                NumberOfPages = book.NumberOfPages
            };

            return bookDto;
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBookAsync(CreateBookDto bookDto)
        {
            Book book = new Book()
            {
                Id = Guid.NewGuid(),
                Title = bookDto.Title,
                Author = bookDto.Author,
                Year = bookDto.Year,
                NumberOfPages = bookDto.NumberOfPages
            };

            await _libraryItemService.CreateBookAsync(book);

            
            return CreatedAtAction(nameof(GetBookAsync), new { bookId = book.Id },
                new
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Year = book.Year,
                    NumberOfPages = book.NumberOfPages
                });
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult> UpdateBookAsync(Guid bookId, UpdateBookDto bookDto)
        {
            var existingBook = await _libraryItemService.GetBookAsync(bookId);

            if(existingBook == null)
            {
                return NotFound();
            }

            var updatedBook = new Book
            {
                Id = existingBook.Id,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Year = bookDto.Year,
                NumberOfPages = bookDto.NumberOfPages
            };

            await _libraryItemService.UpdateBookAsync(updatedBook);

            return NoContent();
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBookAsync(Guid bookId)
        {
            var book = await _libraryItemService.GetBookAsync(bookId);

            if(book == null)
            {
                return NotFound();
            }

            await _libraryItemService.DeleteBookAsync(bookId);
            return NoContent();
        }

        [HttpPatch("{bookId:guid}")]
        public async Task<ActionResult> PatchBookAsync(Guid bookId, [FromBody] JsonPatchDocument<Book> patchDoc)
        {
            if(patchDoc != null)
            {
                var book = await _libraryItemService.GetBookAsync(bookId);

                if (book == null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(book, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _libraryItemService.UpdateBookAsync(book);

                var bookDto = new UpdateBookDto
                {
                    Title = book.Title,
                    Author = book.Author,
                    Year = book.Year,
                    NumberOfPages = book.NumberOfPages
                };

                return new ObjectResult(bookDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
