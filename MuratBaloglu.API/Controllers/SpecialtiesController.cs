using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Models.Blogs;
using MuratBaloglu.Application.Models.Common;
using MuratBaloglu.Application.Models.Specialties;
using MuratBaloglu.Application.Repositories.SpecialityImageFileRepository;
using MuratBaloglu.Application.Repositories.SpecialityRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Infrastructure.Operations;
using System.Reflection.Metadata;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialityReadRepository _specialityReadRepository;
        private readonly ISpecialityWriteRepository _specialityWriteRepository;
        private readonly ISpecialityImageFileReadRepository _specialityImageFileReadRepository;
        private readonly ISpecialityImageFileWriteRepository _specialityImageFileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public SpecialtiesController(
            ISpecialityReadRepository specialityReadRepository,
            ISpecialityWriteRepository specialityWriteRepository,
            ISpecialityImageFileReadRepository specialityImageFileReadRepository,
            ISpecialityImageFileWriteRepository specialityImageFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration)
        {
            _specialityReadRepository = specialityReadRepository;
            _specialityWriteRepository = specialityWriteRepository;
            _specialityImageFileReadRepository = specialityImageFileReadRepository;
            _specialityImageFileWriteRepository = specialityImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var specialties = await _specialityReadRepository.GetAll(false).ToListAsync();

                return Ok(specialties);
            }

            return BadRequest("Uzmanlıklarım listelenirken bir hata ile karşılaşıldı ...");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSpecialtiesWithCardImage()
        {
            if (ModelState.IsValid)
            {
                var query = _specialityReadRepository.Table
                    .Include(s => s.SpecialityImageFiles)
                    .Select(s => new SpecialityWithCardImageModel
                    {
                        Id = s.Id.ToString(),
                        Title = s.Title,
                        Context = s.Context,
                        CardContext = s.CardContext,
                        DetailUrl = s.DetailUrl,
                        //CreatedDate = s.CreatedDate,
                        SpecialityImageFileId = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Id.ToString(),
                        FileName = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).FileName,
                        Path = $"{_configuration["BaseStorageUrl"]}/{s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Path}"
                    }).OrderBy(x => x.Title);

                var specialties = await query.ToListAsync();

                return Ok(specialties);
            }

            return BadRequest("Uzmanlıklarım listelenirken bir hata ile karşılaşıldı ...");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetSpecialityDetail(string id)
        {
            if (ModelState.IsValid)
            {
                var titlesAndDetailUrls = await _specialityReadRepository.GetAll(false).Select(s => new TitleAndDetailUrlModel
                {
                    Title = s.Title,
                    DetailUrl = s.DetailUrl
                }).ToListAsync();

                var speciality = await _specialityReadRepository.GetByIdAsync(id);

                var specialityDetailModel = new SpecialityDetailModel
                {
                    TitlesAndDetailUrls = titlesAndDetailUrls,
                    Title = speciality.Title,
                    Context = speciality.Context
                };

                return Ok(specialityDetailModel);
            }

            return BadRequest();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetSpecialityById(string id)
        {
            var speciality = await _specialityReadRepository.GetByIdAsync(id);
            if (speciality is null)
            {
                return BadRequest("Uzmanlık güncellenirken bir hata ile karşılaşıldı ...");
            }
            return Ok(speciality);
        }

        [HttpGet("[action]/{detailUrl}")]
        public async Task<IActionResult> GetSpecialityDetailByDetailUrl(string detailUrl)
        {
            if (ModelState.IsValid)
            {
                var titlesAndDetailUrls = await _specialityReadRepository.GetAll(false).Select(s => new TitleAndDetailUrlModel
                {
                    Title = s.Title,
                    DetailUrl = s.DetailUrl
                }).ToListAsync();

                var speciality = await _specialityReadRepository.GetSingleAsync(s => s.DetailUrl == detailUrl);

                var specialityDetailModel = new SpecialityDetailModel
                {
                    TitlesAndDetailUrls = titlesAndDetailUrls,
                    Title = speciality.Title,
                    Context = speciality.Context
                };

                return Ok(specialityDetailModel);
            }

            return BadRequest();
        }

        [HttpGet("[action]")] //Bu Method Uzmanlıklarım Menüsü altındaki title listesini göstermek ve footor hızlı bağlantılar için oluşturuldu.
        public async Task<IActionResult> GetSpecialityTitlesAndDetailUrls()
        {
            if (ModelState.IsValid)
            {
                var titlesAndDetailUrls = await _specialityReadRepository.GetAll(false).Select(s => new TitleAndDetailUrlModel
                {
                    Title = s.Title,
                    DetailUrl = s.DetailUrl
                }).ToListAsync();

                return Ok(titlesAndDetailUrls);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post(SpecialityAddModel specialityAddModel)
        {
            if (ModelState.IsValid)
            {
                Speciality speciality = new Speciality
                {
                    Title = specialityAddModel.Title.Trim(),
                    Context = specialityAddModel.Context,
                    CardContext = specialityAddModel.CardContext,
                    DetailUrl = NameRegulatoryOperation.RegulateCharacters(specialityAddModel.Title)
                };

                while (speciality.CardContext.EndsWith("."))
                {
                    speciality.CardContext = speciality.CardContext.Substring(0, speciality.CardContext.Length - 1);
                }

                bool hasSpeciality = await _specialityReadRepository.GetWhere(s => s.Title.Contains(speciality.Title) || s.DetailUrl.Contains(speciality.DetailUrl)).AnyAsync();

                if (!hasSpeciality)
                {
                    await _specialityWriteRepository.AddAsync(speciality);
                    await _specialityWriteRepository.SaveAsync();

                    return Ok(speciality);
                }
                else
                    return BadRequest("Aynı uzmanlık başlığına sahip zaten bir uzmanlık var. Ya uzmanlık başlığını değiştiriniz yada aynı başlığa sahip uzmanlığı siliniz ...");
            }

            return BadRequest("Uzmanlık Oluşturulurken bir hata ile karşılaşıldı ...");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var isRemoved = await _specialityWriteRepository.RemoveAsync(id);
                if (isRemoved)
                {
                    var specialityImageFiles = _specialityImageFileReadRepository.GetWhere(sif => sif.SpecialityId == Guid.Parse(id));
                    if (!specialityImageFiles.IsNullOrEmpty())
                        await specialityImageFiles.ForEachAsync(sif => _storageService.DeleteAsync("resources", sif.FileName));

                    await _specialityWriteRepository.SaveAsync();

                    return Ok(new { Message = "Silme işlemi başarı ile gerçekleşmiştir." });

                    //frontend tarafinda data null olarak karşılanır. (next ile yakalanan data)
                    //return Ok();
                }
            }

            return BadRequest("Silme aşamasında bir sorun ile karşılaşıldı..");
        }

        [HttpPut]
        public async Task<IActionResult> Put(SpecialityUpdateModel specialityUpdateModel)
        {
            if (ModelState.IsValid)
            {
                Speciality speciality = await _specialityReadRepository.GetByIdAsync(specialityUpdateModel.Id);
                speciality.Title = specialityUpdateModel.Title.Trim();
                speciality.CardContext = specialityUpdateModel.CardContext;
                speciality.Context = specialityUpdateModel.Context;

                await _specialityWriteRepository.SaveAsync();

                return Ok(speciality);
            }

            return BadRequest("Uzmanlık güncellenirken bir hata ile karşılaşıldı ...");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadSpecialityImageForSpecialityCard(string id)
        {
            var specialityImageFiles = _specialityImageFileReadRepository.GetWhere(sif => sif.SpecialityId == Guid.Parse(id) && sif.IsSpecialityCardImage);

            if (!specialityImageFiles.IsNullOrEmpty())
            {
                await specialityImageFiles.ForEachAsync(sif => _specialityImageFileWriteRepository.Remove(sif));
                await specialityImageFiles.ForEachAsync(sif => _storageService.DeleteAsync("resources", sif.FileName));
            }

            var result = await _storageService.UploadAsync("resources", Request.Form.Files);

            Speciality speciality = await _specialityReadRepository.GetByIdAsync(id);
            result.ForEach(r =>
            {
                speciality.SpecialityImageFiles.Add(new SpecialityImageFile()
                {
                    FileName = r.fileName,
                    Path = r.pathOrContainerNameIncludeFileName,
                    Storage = _storageService.StorageName,
                    IsSpecialityCardImage = true
                });
            });

            await _specialityWriteRepository.SaveAsync();

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            var result = await _storageService.UploadAsync("resources", Request.Form.Files);

            Speciality speciality = await _specialityReadRepository.GetByIdAsync(id);

            await _specialityImageFileWriteRepository.AddRangeAsync(result.Select(r => new SpecialityImageFile()
            {
                FileName = r.fileName,
                Path = r.pathOrContainerNameIncludeFileName,
                Storage = _storageService.StorageName,
                Speciality = speciality
            }).ToList());

            await _specialityImageFileWriteRepository.SaveAsync();

            return Ok();
        }

    }
}
