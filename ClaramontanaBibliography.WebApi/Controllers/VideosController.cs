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
    public class VideosController : ControllerBase
    {
        private readonly ILibraryItemService _libraryItemService;
        public VideosController(ILibraryItemService libraryItemService)
        {
            _libraryItemService = libraryItemService;
        }


        [HttpGet]
        public async Task<IEnumerable<VideoDto>> GetAllVideosAsync()
        {
            var videos = (await _libraryItemService.GetAllVideosAsync()).Select(x => new VideoDto
            {
                Id = x.Id,
                Title = x.Title,
                Director = x.Director,
                Year = x.Year,
                ImageUrl = x.ImageUrl
            });
            return videos;
        }
    }
}
