using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Models.AboutMe;
using MuratBaloglu.Application.Repositories.AboutMeImageFileRepository;
using MuratBaloglu.Application.Repositories.AboutMeRepository;
using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutMeController : ControllerBase
    {
        private readonly IAboutMeReadRepository _aboutMeReadRepository;
        private readonly IAboutMeWriteRepository _aboutMeWriteRepository;
        private readonly IAboutMeImageFileReadRepository _aboutMeImageFileReadRepository;
        private readonly IAboutMeImageFileWriteRepository _aboutMeImageFileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public AboutMeController(
            IAboutMeReadRepository aboutMeReadRepository,
            IAboutMeWriteRepository aboutMeWriteRepository,
            IAboutMeImageFileReadRepository aboutMeImageFileReadRepository,
            IAboutMeImageFileWriteRepository aboutMeImageFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration
            )
        {
            _aboutMeReadRepository = aboutMeReadRepository;
            _aboutMeWriteRepository = aboutMeWriteRepository;
            _aboutMeImageFileReadRepository = aboutMeImageFileReadRepository;
            _aboutMeImageFileWriteRepository = aboutMeImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var aboutMe = await _aboutMeReadRepository.GetAll(false).Select(about => new
                {
                    about.Context,
                    about.HomeContext
                }).FirstOrDefaultAsync();

                return Ok(aboutMe);
            }

            return BadRequest("Hakkında içeriği getirilemiyor ...");
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AboutMe, ActionType = ActionType.Writing, Definition = "Hakkında Ekleme veya Güncelleme")]
        public async Task<IActionResult> AddAboutMe(AboutMeAddModel aboutMeAddModel)
        {
            if (ModelState.IsValid)
            {
                var query = await _aboutMeReadRepository.GetAll(false).FirstOrDefaultAsync();

                if (query is null)
                {
                    AboutMe aboutMe = new AboutMe()
                    {
                        Context = aboutMeAddModel.Context
                    };

                    await _aboutMeWriteRepository.AddAsync(aboutMe);
                    await _aboutMeWriteRepository.SaveAsync();
                    return Ok(aboutMeAddModel);
                }

                query.Context = aboutMeAddModel.Context;

                _aboutMeWriteRepository.Update(query);
                await _aboutMeWriteRepository.SaveAsync();
                return Ok(aboutMeAddModel);
            }

            return BadRequest(new { Message = "Hakkında eklenirken bir hata ile karşılaşıldı." });
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AboutMe, ActionType = ActionType.Writing, Definition = "Anasayfa Hakkında Ekleme veya Güncelleme")]
        public async Task<IActionResult> AddHomeAboutMe(AboutMeAddModel aboutMeAddModel)
        {
            if (ModelState.IsValid)
            {
                var query = await _aboutMeReadRepository.GetAll(false).FirstOrDefaultAsync();

                if (query is null)
                {
                    AboutMe aboutMe = new AboutMe()
                    {
                        HomeContext = aboutMeAddModel.HomeContext
                    };

                    await _aboutMeWriteRepository.AddAsync(aboutMe);
                    await _aboutMeWriteRepository.SaveAsync();
                    return Ok(aboutMeAddModel);
                }

                query.HomeContext = aboutMeAddModel.HomeContext;

                _aboutMeWriteRepository.Update(query);
                await _aboutMeWriteRepository.SaveAsync();
                return Ok(aboutMeAddModel);
            }

            return BadRequest(new { Message = "Anasayfadaki hakkında yazısı eklenirken bir hata ile karşılaşıldı." });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAboutMeWithImage()
        {
            var query = await _aboutMeReadRepository.Table
                .Include(a => a.AboutMeImageFile)
                .Select(a => new AboutMeWithImage
                {
                    HomeContext = a.HomeContext,
                    AboutMeImageFileId = a.AboutMeImageFile.Id.ToString(),
                    FileName = a.AboutMeImageFile.FileName,
                    Path = $"{_configuration["BaseStorageUrl"]}/{a.AboutMeImageFile.Path}"
                }).FirstOrDefaultAsync();

            return Ok(query);
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AboutMe, ActionType = ActionType.Writing, Definition = "Anasayfa Hakkında Resmi Ekle Yada Değiştirme")]
        public async Task<IActionResult> Upload()
        {
            var query = await _aboutMeReadRepository.GetAll(false).FirstOrDefaultAsync();

            if (query is null)
                return BadRequest("Hakkında resmi yüklenebilmesi için ilk önce hakkında içeriği oluşturulmalıdır.");

            var aboutMeImageFile = await _aboutMeImageFileReadRepository.GetSingleAsync(aif => aif.AboutMeId == query.Id);

            if (aboutMeImageFile is not null)
            {
                _aboutMeImageFileWriteRepository.Remove(aboutMeImageFile);
                await _storageService.DeleteAsync("resources", aboutMeImageFile.FileName);
            }

            var result = await _storageService.UploadAsync("resources", Request.Form.Files);

            result.ForEach(r =>
            {
                query.AboutMeImageFile = new AboutMeImageFile()
                {
                    FileName = r.fileName,
                    Path = r.pathOrContainerNameIncludeFileName,
                    Storage = _storageService.StorageName
                };
            });

            _aboutMeWriteRepository.Update(query);
            await _aboutMeWriteRepository.SaveAsync();

            return Ok();

        }
    }
}
