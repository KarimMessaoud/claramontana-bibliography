using ClaramontanaBibliography.Service;
using ClaramontanaBibliography.WebApi.Dtos;
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
                ImageUrl = x.ImageUrl
            });
            return books;
        }

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
                ImageUrl = book.ImageUrl
            };

            return bookDto;
        }
    }
}
