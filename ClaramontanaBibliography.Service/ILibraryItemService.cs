using ClaramontanaBibliography.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service
{
    public interface ILibraryItemService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Video>> GetAllVideosAsync();
        Task<Book> GetBookAsync(Guid bookId);
        Task<Video> GetVideoAsync(Guid videoId);
        Task CreateBookAsync(Book book);
        
    }
}