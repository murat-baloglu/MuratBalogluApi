using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Repositories.CarouselImageFileRepository;
using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ICarouselImageFileReadRepository _carouselImageFileReadRepository;
        private readonly ICarouselImageFileWriteRepository _carouselImageFileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public HomeController(ICarouselImageFileReadRepository carouselImageFileReadRepository,
            ICarouselImageFileWriteRepository carouselImageFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration)
        {
            _carouselImageFileReadRepository = carouselImageFileReadRepository;
            _carouselImageFileWriteRepository = carouselImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCarouselImages()
        {
            if (ModelState.IsValid)
            {
                var query = await _carouselImageFileReadRepository.GetAll(false).OrderBy(cif => cif.CreatedDate)
                    .Select(cif => new
                    {
                        cif.Id,
                        cif.FileName,
                        Path = $"{_configuration["BaseStorageUrl"]}/{cif.Path}"
                    }).ToListAsync();

                return Ok(query);
            }

            return BadRequest(new { Message = "Carousel resimleri listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpPost("[action]")] //Carousel resimlerini yüklemek için bu action ı kullanıyoruz.
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Home, ActionType = ActionType.Writing, Definition = "Carousel / Slider Resimlerini Yükleme")]
        public async Task<IActionResult> UploadCarouselImages()
        {
            var result = await _storageService.UploadAsync("resources", Request.Form.Files);

            await _carouselImageFileWriteRepository.AddRangeAsync(result.Select(r => new CarouselImageFile()
            {
                FileName = r.fileName,
                Path = r.pathOrContainerNameIncludeFileName,
                Storage = _storageService.StorageName
            }).ToList());

            await _carouselImageFileWriteRepository.SaveAsync();

            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Home, ActionType = ActionType.Deleting, Definition = "Carousel / Slider Resimini Silme")]
        public async Task<IActionResult> DeleteCarouselImage(string id, string fileName)
        {
            if (ModelState.IsValid)
            {
                var isRemoved = await _carouselImageFileWriteRepository.RemoveAsync(id);
                if (isRemoved)
                {
                    await _storageService.DeleteAsync("resources", fileName);

                    await _carouselImageFileWriteRepository.SaveAsync();
                    return Ok(new { Message = "Silme işlemi gerçekleşmiştir." });
                }
            }

            return BadRequest(new { Message = "Silme aşamasında bir sorun ile karşılaşıldı." });
        }
    }
}
