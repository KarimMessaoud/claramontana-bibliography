using ClaramontanaBibliography.Data.Entities;
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
                DurationInMinutes = x.DurationInMinutes
            });

            return videos;
        }

        [ActionName("GetVideoAsync")]
        [HttpGet("{videoId:guid}")]
        public async Task<ActionResult<VideoDto>> GetVideoAsync(Guid videoId)
        {
            var video = await _libraryItemService.GetVideoAsync(videoId);

            if (video == null)
            {
                return NotFound();
            }

            var videoDto = new VideoDto
            {
                Id = video.Id,
                Title = video.Title,
                Director = video.Director,
                Year = video.Year,
                DurationInMinutes = video.DurationInMinutes
            };

            return videoDto;
        }

        [HttpPost]
        public async Task<ActionResult<VideoDto>> CreateVideoAsync(CreateVideoDto videoDto)
        {
            Video video = new Video()
            {
                Id = Guid.NewGuid(),
                Title = videoDto.Title,
                Director = videoDto.Director,
                Year = videoDto.Year,
                DurationInMinutes = videoDto.DurationInMinutes
            };

            await _libraryItemService.CreateVideoAsync(video);


            return CreatedAtAction(nameof(GetVideoAsync), new { videoId = video.Id },
                new
                {
                    Id = video.Id,
                    Title = video.Title,
                    Author = video.Director,
                    Year = video.Year,
                    DurationInMinutes = video.DurationInMinutes
                });
        }

        [HttpPut("{videoId}")]
        public async Task<ActionResult> UpdateVideoAsync(Guid videoId, UpdateVideoDto videoDto)
        {
            var existingVideo = await _libraryItemService.GetVideoAsync(videoId);

            if (existingVideo == null)
            {
                return NotFound();
            }

            var updatedVideo = new Video
            {
                Id = existingVideo.Id,
                Title = videoDto.Title,
                Director = videoDto.Director,
                Year = videoDto.Year,
                DurationInMinutes = videoDto.DurationInMinutes
            };

            await _libraryItemService.UpdateVideoAsync(updatedVideo);

            return NoContent();
        }
    }
}
