using ClaramontanaBibliography.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service
{
    public class LibraryItemService : ILibraryItemService
    {
        private readonly LibraryContext _libraryContext;
        public LibraryItemService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var books = await _libraryContext.Books.ToListAsync();
            return books;
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            var videos = await _libraryContext.Videos.ToListAsync();
            return videos;
        }

        public async Task<Book> GetBookAsync(Guid bookId)
        {

            var book = await _libraryContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            return book;
        }       
    }           
}
