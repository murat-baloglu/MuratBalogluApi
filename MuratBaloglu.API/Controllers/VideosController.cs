using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Models.Videos;
using MuratBaloglu.Application.Repositories.VideoRepository;
using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoReadRepository _videoReadRepository;
        private readonly IVideoWriteRepository _videoWriteRepository;

        public VideosController(IVideoReadRepository videoReadRepository, IVideoWriteRepository videoWriteRepository)
        {
            _videoReadRepository = videoReadRepository;
            _videoWriteRepository = videoWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var videos = await _videoReadRepository.GetAll(false).ToListAsync();
                return Ok(videos);
            }

            return BadRequest(new { Message = "Videolar listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLastNineVideo()
        {
            if (ModelState.IsValid)
            {
                var videos = await _videoReadRepository.GetAll(false).OrderByDescending(v => v.CreatedDate).Take(9).ToListAsync();
                return Ok(videos);
            }

            return BadRequest(new { Message = "Videolar listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Videos, ActionType = ActionType.Writing, Definition = "Video Ekleme")]
        public async Task<IActionResult> Post(VideoAddModel videoAddModel)
        {
            if (ModelState.IsValid)
            {
                Video video = new Video()
                {
                    Title = videoAddModel.Title.Trim(),
                    Link = videoAddModel.Link,
                };

                bool hasVideo = await _videoReadRepository.GetWhere(v => v.Title.Contains(video.Title)).AnyAsync();
                if (!hasVideo)
                {
                    await _videoWriteRepository.AddAsync(video);
                    await _videoWriteRepository.SaveAsync();
                    return Ok(video);
                }
                else
                    return BadRequest(new { Message = "Aynı video başlığına sahip zaten bir video var." });
            }

            return BadRequest(new { Message = "Video Eklenirken bir hata ile karşılaşıldı." });
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Videos, ActionType = ActionType.Deleting, Definition = "Video Silme")]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                await _videoWriteRepository.RemoveAsync(id);
                await _videoWriteRepository.SaveAsync();
                return Ok(new { Message = "Silme işlemi başarı ile gerçekleşmiştir." });
            }

            return BadRequest(new { Message = "Silme aşamasında bir sorun ile karşılaşıldı." });
        }
    }
}
