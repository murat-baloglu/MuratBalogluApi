using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Models.News;
using MuratBaloglu.Application.Repositories.NewsImageFileRepository;
using MuratBaloglu.Application.Repositories.NewsRepository;
using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsReadRepository _newsReadRepository;
        private readonly INewsWriteRepository _newsWriteRepository;
        private readonly INewsImageFileReadRepository _newsImageFileReadRepository;
        private readonly INewsImageFileWriteRepository _newsImageFileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public NewsController(
            INewsReadRepository newsReadRepository,
            INewsWriteRepository newsWriteRepository,
            INewsImageFileReadRepository newsImageFileReadRepository,
            INewsImageFileWriteRepository newsImageFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration
            )
        {
            _newsReadRepository = newsReadRepository;
            _newsWriteRepository = newsWriteRepository;
            _newsImageFileReadRepository = newsImageFileReadRepository;
            _newsImageFileWriteRepository = newsImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.News, ActionType = ActionType.Writing, Definition = "Basından Ekleme")]
        public async Task<IActionResult> Post(NewsAddModel newsAddModel)
        {
            if (ModelState.IsValid)
            {
                News news = new News
                {
                    Title = newsAddModel.Title.Trim(),
                    Link = newsAddModel.Link,
                };

                bool hasNews = await _newsReadRepository.GetWhere(n => n.Title.Contains(news.Title)).AnyAsync();

                if (!hasNews)
                {
                    await _newsWriteRepository.AddAsync(news);
                    await _newsWriteRepository.SaveAsync();

                    return Ok(news);
                }
                else
                    return BadRequest(new { Message = "Aynı basın başlığına sahip zaten bir haber var." });
            }

            return BadRequest(new { Message = "Basın Oluşturulurken bir hata ile karşılaşıldı." });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetNewsWithCardImage()
        {
            if (ModelState.IsValid)
            {
                var query = _newsReadRepository.Table
                    .Include(n => n.NewsImageFile)
                    .Select(n => new NewsWithCardImageModel
                    {
                        Id = n.Id.ToString(),
                        Title = n.Title,
                        Link = n.Link,
                        CreatedDate = n.CreatedDate,
                        NewsImageFileId = n.NewsImageFile.Id.ToString(),
                        FileName = n.NewsImageFile.FileName,
                        Path = $"{_configuration["BaseStorageUrl"]}/{n.NewsImageFile.Path}"
                    }).OrderByDescending(x => x.CreatedDate);

                var news = await query.ToListAsync();

                return Ok(news);
            }

            return BadRequest(new { Message = "Basından listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.News, ActionType = ActionType.Updating, Definition = "Basından Güncelleme")]
        public async Task<IActionResult> Put(NewsUpdateModel newsUpdateModel)
        {
            if (ModelState.IsValid)
            {
                News news = await _newsReadRepository.GetByIdAsync(newsUpdateModel.Id);
                news.Title = newsUpdateModel.Title.Trim();
                news.Link = newsUpdateModel.Link;

                await _newsWriteRepository.SaveAsync();

                return Ok(news);
            }

            return BadRequest(new { Message = "Basından güncellenirken bir hata ile karşılaşıldı." });
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.News, ActionType = ActionType.Deleting, Definition = "Basından Silme")]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var isRemoved = await _newsWriteRepository.RemoveAsync(id);
                if (isRemoved)
                {
                    var newsImageFile = await _newsImageFileReadRepository.GetSingleAsync(nif => nif.NewsId == Guid.Parse(id));
                    if (newsImageFile is not null)
                        await _storageService.DeleteAsync("resources", newsImageFile.FileName);

                    await _newsWriteRepository.SaveAsync();

                    return Ok(new { Message = "Silme işlemi başarı ile gerçekleşmiştir." });
                }
            }

            return BadRequest(new { Message = "Silme aşamasında bir sorun ile karşılaşıldı." });
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.News, ActionType = ActionType.Writing, Definition = "Haber İçin Resim Yükleme")]
        public async Task<IActionResult> UploadNewsImageForNews(string id)
        {
            var newsImageFile = await _newsImageFileReadRepository.GetSingleAsync(nif => nif.NewsId == Guid.Parse(id));

            if (newsImageFile is not null)
            {
                _newsImageFileWriteRepository.Remove(newsImageFile);
                await _storageService.DeleteAsync("resources", newsImageFile.FileName);
            }

            var result = await _storageService.UploadAsync("resources", Request.Form.Files);

            News news = await _newsReadRepository.GetByIdAsync(id);
            result.ForEach(r =>
            {
                news.NewsImageFile = new NewsImageFile()
                {
                    FileName = r.fileName,
                    Path = r.pathOrContainerNameIncludeFileName,
                    Storage = _storageService.StorageName
                };
            });

            //_newsWriteRepository.Update(news);
            await _newsWriteRepository.SaveAsync();

            return Ok();
        }
    }
}
