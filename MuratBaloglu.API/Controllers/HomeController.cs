using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Services.Caching;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Models.Home;
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
        private readonly ICacheService _cacheService;

        public HomeController(ICarouselImageFileReadRepository carouselImageFileReadRepository,
            ICarouselImageFileWriteRepository carouselImageFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration,
            ICacheService cacheService)
        {
            _carouselImageFileReadRepository = carouselImageFileReadRepository;
            _carouselImageFileWriteRepository = carouselImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _cacheService = cacheService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCarouselImages()
        {
            if (ModelState.IsValid)
            {
                List<CarouselImageModel>? response = await _cacheService.GetAsync<List<CarouselImageModel>>("carouselImages");

                if (response is not null)
                {
                    return Ok(response);
                }

                List<CarouselImageModel>? carouselImages = await _carouselImageFileReadRepository
                    .GetAll(false)
                    .OrderBy(cif => cif.CreatedDate)
                    .Select(cif => new CarouselImageModel()
                    {
                        Id = cif.Id,
                        FileName = cif.FileName,
                        Path = $"{_configuration["BaseStorageUrl"]}/{cif.Path}"
                    }).ToListAsync();

                response = carouselImages;

                await _cacheService.SetAsync("carouselImages", response);

                return Ok(response);

                //Alternatif Yol
                //await _cacheService.GetAsync("carouselImages", async () =>
                //{
                //    List<CarouselImageModel> carouselImages = await _carouselImageFileReadRepository
                //        .GetAll(false)
                //        .OrderBy(cif => cif.CreatedDate)
                //        .Select(cif => new CarouselImageModel()
                //        {
                //            Id = cif.Id,
                //            FileName = cif.FileName,
                //            Path = $"{_configuration["BaseStorageUrl"]}/{cif.Path}"
                //        }).ToListAsync();

                //    return Ok(carouselImages);
                //});                
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

            await _cacheService.RemoveAsync("carouselImages");

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

                    await _cacheService.RemoveAsync("carouselImages");

                    return Ok(new { Message = "Silme işlemi gerçekleşmiştir." });
                }
            }

            return BadRequest(new { Message = "Silme aşamasında bir sorun ile karşılaşıldı." });
        }
    }
}
