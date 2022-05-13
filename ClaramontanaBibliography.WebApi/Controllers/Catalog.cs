using ClaramontanaBibliography.Data.Models;
using ClaramontanaBibliography.Service;
using ClaramontanaBibliography.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Catalog : ControllerBase
    {
        private readonly ILibraryItemService _libraryItemService;
        public Catalog(ILibraryItemService libraryItemService)
        {
            _libraryItemService = libraryItemService;
        }

        [HttpGet]
        [Route("Books")]
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
    }
}
