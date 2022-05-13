using ClaramontanaBibliography.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service
{
    public interface ILibraryItemService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Video>> GetAllVideosAsync();
    }
}