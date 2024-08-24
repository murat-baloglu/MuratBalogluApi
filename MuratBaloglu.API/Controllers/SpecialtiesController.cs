using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Models.Common;
using MuratBaloglu.Application.Models.SpecialityCategory;
using MuratBaloglu.Application.Models.Specialties;
using MuratBaloglu.Application.Repositories.SpecialityCategoryRepository;
using MuratBaloglu.Application.Repositories.SpecialityImageFileRepository;
using MuratBaloglu.Application.Repositories.SpecialityRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Infrastructure.Operations;

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
        private readonly ISpecialityCategoryReadRepository _specialityCategoryReadRepository;
        private readonly ISpecialityCategoryWriteRepository _specialityCategoryWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public SpecialtiesController(
            ISpecialityReadRepository specialityReadRepository,
            ISpecialityWriteRepository specialityWriteRepository,
            ISpecialityImageFileReadRepository specialityImageFileReadRepository,
            ISpecialityImageFileWriteRepository specialityImageFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration,
            ISpecialityCategoryReadRepository specialityCategoryReadRepository,
            ISpecialityCategoryWriteRepository specialityCategoryWriteRepository)
        {
            _specialityReadRepository = specialityReadRepository;
            _specialityWriteRepository = specialityWriteRepository;
            _specialityImageFileReadRepository = specialityImageFileReadRepository;
            _specialityImageFileWriteRepository = specialityImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _specialityCategoryReadRepository = specialityCategoryReadRepository;
            _specialityCategoryWriteRepository = specialityCategoryWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var specialties = await _specialityReadRepository.GetAll(false).ToListAsync();

                return Ok(specialties);
            }

            return BadRequest(new { Message = "Uzmanlıklarım listelenirken bir hata ile karşılaşıldı." });
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

            return BadRequest(new { Message = "Uzmanlıklarım listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLastNineSpecialityWithCardImage()
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
                       CreatedDate = s.CreatedDate,
                       SpecialityImageFileId = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Id.ToString(),
                       FileName = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).FileName,
                       Path = $"{_configuration["BaseStorageUrl"]}/{s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Path}"
                   }).OrderByDescending(x => x.CreatedDate).Take(9);

                var specialties = await query.ToListAsync();

                return Ok(specialties);
            }

            return BadRequest(new { Message = "Uzmanlıklarım listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSpecialtiesByCategoryIdWithCardImage(string categoryId)
        {
            if (ModelState.IsValid)
            {
                var query = _specialityReadRepository.Table
                    .Include(s => s.SpecialityImageFiles)
                    .Where(s => s.CategoryId == Guid.Parse(categoryId))
                    .Select(s => new SpecialityWithCardImageModel
                    {
                        Id = s.Id.ToString(),
                        Title = s.Title,
                        Context = s.Context,
                        CardContext = s.CardContext,
                        //CategoryName = s.Category.Name,
                        DetailUrl = s.DetailUrl,
                        //CreatedDate = s.CreatedDate,
                        SpecialityImageFileId = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Id.ToString(),
                        FileName = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).FileName,
                        Path = $"{_configuration["BaseStorageUrl"]}/{s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Path}"
                    }).OrderBy(x => x.Title);

                var specialties = await query.ToListAsync();

                return Ok(specialties);
            }

            return BadRequest(new { Message = "Uzmanlıklarım listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpGet("[action]/{categoryUrl}")]
        public async Task<IActionResult> GetSpecialtiesByCategoryWithCardImage(string categoryUrl)
        {
            if (ModelState.IsValid)
            {
                var query = _specialityReadRepository.Table
                    .Include(s => s.SpecialityImageFiles)
                    .Include(s => s.Category)
                    .Where(s => s.Category.CategoryUrl == categoryUrl)
                    .Select(s => new SpecialityWithCardImageModel
                    {
                        Id = s.Id.ToString(),
                        Title = s.Title,
                        Context = s.Context,
                        CardContext = s.CardContext,
                        CategoryName = s.Category.Name,
                        DetailUrl = s.DetailUrl,
                        //CreatedDate = s.CreatedDate,
                        SpecialityImageFileId = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Id.ToString(),
                        FileName = s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).FileName,
                        Path = $"{_configuration["BaseStorageUrl"]}/{s.SpecialityImageFiles.Single(sif => sif.IsSpecialityCardImage).Path}"
                    }).OrderBy(x => x.Title);

                var specialties = await query.ToListAsync();

                return Ok(specialties);
            }

            return BadRequest(new { Message = "Uzmanlıklarım listelenirken bir hata ile karşılaşıldı." });
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
            var query = await _specialityReadRepository.Table
                .Include(s => s.Category)
                .Select(s => new
                {
                    Id = s.Id,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category.Name,
                    Title = s.Title,
                    Context = s.Context,
                    CardContext = s.CardContext,
                    DetailUrl = s.DetailUrl,
                    CreatedDate = s.CreatedDate,
                    UpdatedDate = s.UpdatedDate
                })
                .FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));

            if (query is null)
            {
                return BadRequest(new { Message = "İlgili uzmanlık getirilemiyor." });
            }

            return Ok(query);

            //var speciality = await _specialityReadRepository.GetByIdAsync(id);
            //if (speciality is null)
            //{
            //    return BadRequest("Uzmanlık güncellenirken bir hata ile karşılaşıldı ...");
            //}
            //return Ok(speciality);
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
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Specialties, ActionType = ActionType.Writing, Definition = "Uzmanlık Ekleme")]
        public async Task<IActionResult> Post(SpecialityAddModel specialityAddModel)
        {
            if (ModelState.IsValid)
            {
                Speciality speciality = new Speciality
                {
                    CategoryId = Guid.Parse(specialityAddModel.CategoryId),
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
                    return BadRequest(new { Message = "Aynı uzmanlık başlığına sahip zaten bir uzmanlık var." });
            }

            return BadRequest(new { Message = "Uzmanlık Oluşturulurken bir hata ile karşılaşıldı." });
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Specialties, ActionType = ActionType.Deleting, Definition = "Uzmanlık Silme")]
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

            return BadRequest(new { Message = "Silme aşamasında bir sorun ile karşılaşıldı." });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Specialties, ActionType = ActionType.Updating, Definition = "Uzmanlık Güncelleme")]
        public async Task<IActionResult> Put(SpecialityUpdateModel specialityUpdateModel)
        {
            if (ModelState.IsValid)
            {
                Speciality speciality = await _specialityReadRepository.GetByIdAsync(specialityUpdateModel.Id);
                speciality.CategoryId = Guid.Parse(specialityUpdateModel.CategoryId);
                speciality.Title = specialityUpdateModel.Title.Trim();
                speciality.CardContext = specialityUpdateModel.CardContext;
                speciality.Context = specialityUpdateModel.Context;
                speciality.DetailUrl = NameRegulatoryOperation.RegulateCharacters(specialityUpdateModel.Title);

                await _specialityWriteRepository.SaveAsync();

                return Ok(speciality);
            }

            return BadRequest(new { Message = "Uzmanlık güncellenirken bir hata ile karşılaşıldı." });
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Specialties, ActionType = ActionType.Writing, Definition = "Uzmanlık Kategorisi Ekleme")]
        public async Task<IActionResult> AddSpecialityCategory(SpecialityCategoryAddModel specialityCategoryAddModel)
        {
            if (ModelState.IsValid)
            {
                SpecialityCategory specialityCategory = new SpecialityCategory()
                {
                    Name = specialityCategoryAddModel.Name.Trim(),
                    CategoryUrl = NameRegulatoryOperation.RegulateCharacters(specialityCategoryAddModel.Name)
                };

                bool hasSpecialityCategory = await _specialityCategoryReadRepository.GetWhere(sc => sc.Name.Contains(specialityCategory.Name) || sc.CategoryUrl.Contains(specialityCategory.CategoryUrl)).AnyAsync();

                if (!hasSpecialityCategory)
                {
                    await _specialityCategoryWriteRepository.AddAsync(specialityCategory);
                    await _specialityCategoryWriteRepository.SaveAsync();

                    return Ok(specialityCategory);
                }
                else
                    return BadRequest(new { Message = "Aynı uzmanlık kategorine sahip zaten bir kategori var." });
            }

            return BadRequest(new { Message = "Uzmanlık kategorisi eklenirken bir hata ile karşılaşıldı." });
        }

        [HttpPut("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Specialties, ActionType = ActionType.Updating, Definition = "Uzmanlık Kategorisi Güncelleme")]
        public async Task<IActionResult> UpdateSpecialityCategory(SpecialityCategoryUpdateModel specialityCategoryUpdateModel)
        {
            if (ModelState.IsValid)
            {
                SpecialityCategory? specialityCategory = await _specialityCategoryReadRepository.Table
                    .Include(sc => sc.Specialties)
                    .FirstOrDefaultAsync(sc => sc.Id == Guid.Parse(specialityCategoryUpdateModel.Id));

                if (specialityCategory?.Specialties.Count > 0)
                    return BadRequest(new { Message = "Bu kategoriye ait uzmanlıklar var. Bu yüzden güncelleme yapılamaz." });


                specialityCategory.Name = specialityCategoryUpdateModel.Name.Trim();
                specialityCategory.CategoryUrl = NameRegulatoryOperation.RegulateCharacters(specialityCategoryUpdateModel.Name);

                await _specialityCategoryWriteRepository.SaveAsync();

                return Ok(specialityCategory);
            }

            return BadRequest(new { Message = "Uzmanlık kategorisi güncellenirken bir hata ile karşılaşıldı." });
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Specialties, ActionType = ActionType.Deleting, Definition = "Uzmanlık Kategorisi Silme")]
        public async Task<IActionResult> DeleteSpecialityCategory(string id)
        {
            if (ModelState.IsValid)
            {
                SpecialityCategory? specialityCategory = await _specialityCategoryReadRepository.Table
                    .Include(sc => sc.Specialties)
                    .FirstOrDefaultAsync(sc => sc.Id == Guid.Parse(id));

                if (specialityCategory?.Specialties.Count > 0)
                    return BadRequest(new { Message = "Bu kategoriye ait uzmanlıklar var. Bu yüzden silme işlemi yapılamaz." });


                await _specialityCategoryWriteRepository.RemoveAsync(id);
                await _specialityCategoryWriteRepository.SaveAsync();
                return Ok(new { Message = "Silme işlemi başarı ile gerçekleşmiştir." });
            }

            return BadRequest(new { Message = "Silme aşamasında bir sorun ile karşılaşıldı." });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSpecialityCategories()
        {
            if (ModelState.IsValid)
            {
                var specialityCategories = await _specialityCategoryReadRepository.GetAll(false).OrderBy(sc => sc.Name).ToListAsync();
                return Ok(specialityCategories);
            }

            return BadRequest(new { Message = "Uzmanlık kategorileri listelenirken bir hata ile karşılaşıldı." });
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Specialties, ActionType = ActionType.Writing, Definition = "Uzmanlık Kartı İçin Resim Yükleme")]
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