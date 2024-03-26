using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Models.Blogs;
using MuratBaloglu.Application.Models.Common;
using MuratBaloglu.Application.Repositories.BlogImageFileRepository;
using MuratBaloglu.Application.Repositories.BlogRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Infrastructure.Operations;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly IBlogImageFileWriteRepository _blogImageFileWriteRepository;
        private readonly IBlogImageFileReadRepository _blogImageFileReadRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public BlogsController(
            IBlogReadRepository blogReadRepository,
            IBlogWriteRepository blogWriteRepository,
            IBlogImageFileWriteRepository blogImageFileWriteRepository,
            IBlogImageFileReadRepository blogImageFileReadRepository,
            IStorageService storageService,
            IConfiguration configuration)
        {
            _blogReadRepository = blogReadRepository;
            _blogWriteRepository = blogWriteRepository;
            _blogImageFileWriteRepository = blogImageFileWriteRepository;
            _blogImageFileReadRepository = blogImageFileReadRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var blogs = await _blogReadRepository.GetAll(false).ToListAsync();

                return Ok(blogs);
            }

            return BadRequest("Bloglar listelenirken bir hata ile karşılaşıldı ...");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBlogsWithCardImage()
        {
            if (ModelState.IsValid)
            {
                var query = _blogReadRepository.Table
                    .Include(b => b.BlogImageFiles)
                    .Select(b => new BlogWithCardImageModel
                    {
                        Id = b.Id.ToString(),
                        Title = b.Title,
                        Context = b.Context,
                        CardContext = b.CardContext,
                        DetailUrl = b.DetailUrl,
                        CreatedDate = b.CreatedDate,
                        BlogImageFileId = b.BlogImageFiles.Single(bif => bif.IsBlogCardImage).Id.ToString(),
                        FileName = b.BlogImageFiles.Single(bif => bif.IsBlogCardImage).FileName,
                        Path = $"{_configuration["BaseStorageUrl"]}/{b.BlogImageFiles.Single(bif => bif.IsBlogCardImage).Path}"
                    }).OrderByDescending(x => x.CreatedDate);

                var blogs = await query.ToListAsync();

                return Ok(blogs);
            }

            return BadRequest("Bloglar listelenirken bir hata ile karşılaşıldı ...");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetBlogDetail(string id)
        {
            if (ModelState.IsValid)
            {
                var titlesAndDetailUrls = await _blogReadRepository.GetAll(false).Select(b => new TitleAndDetailUrlModel
                {
                    Title = b.Title,
                    DetailUrl = b.DetailUrl
                }).ToListAsync();

                var blog = await _blogReadRepository.GetByIdAsync(id);

                var blogDetailModel = new BlogDetailModel
                {
                    TitlesAndDetailUrls = titlesAndDetailUrls,
                    Title = blog.Title,
                    Context = blog.Context
                };

                return Ok(blogDetailModel);
            }

            return BadRequest();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetBlogById(string id)
        {
            var blog = await _blogReadRepository.GetByIdAsync(id);
            if (blog is null)
            {
                return BadRequest("Blog güncellenirken bir hata ile karşılaşıldı ...");
            }
            return Ok(blog);
        }

        [HttpGet("[action]/{detailUrl}")]
        public async Task<IActionResult> GetBlogDetailByDetailUrl(string detailUrl)
        {
            if (ModelState.IsValid)
            {
                var titlesAndDetailUrls = await _blogReadRepository.GetAll(false).Select(b => new TitleAndDetailUrlModel
                {
                    Title = b.Title,
                    DetailUrl = b.DetailUrl
                }).ToListAsync();

                var blog = await _blogReadRepository.GetSingleAsync(b => b.DetailUrl == detailUrl);

                var blogDetailModel = new BlogDetailModel
                {
                    TitlesAndDetailUrls = titlesAndDetailUrls,
                    Title = blog.Title,
                    Context = blog.Context
                };

                return Ok(blogDetailModel);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post(BlogAddModel blogAddModel)
        {
            if (ModelState.IsValid)
            {
                Blog blog = new Blog
                {
                    Title = blogAddModel.Title.Trim(),
                    Context = blogAddModel.Context,
                    CardContext = blogAddModel.CardContext,
                    DetailUrl = NameRegulatoryOperation.RegulateCharacters(blogAddModel.Title)
                };

                while (blog.CardContext.EndsWith("."))
                {
                    blog.CardContext = blog.CardContext.Substring(0, blog.CardContext.Length - 1);
                }

                bool hasBlog = await _blogReadRepository.GetWhere(b => b.Title.Contains(blog.Title) || b.DetailUrl.Contains(blog.DetailUrl)).AnyAsync();

                if (!hasBlog)
                {
                    await _blogWriteRepository.AddAsync(blog);
                    await _blogWriteRepository.SaveAsync();

                    return Ok(blog);
                }
                else
                    return BadRequest("Aynı blog başlığına sahip zaten bir blog var. Ya blog başlığını değiştiriniz yada aynı başlığa sahip bloğu siliniz ...");
            }

            return BadRequest("Blog Oluşturulurken bir hata ile karşılaşıldı ...");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var isRemoved = await _blogWriteRepository.RemoveAsync(id);
                if (isRemoved)
                {
                    var blogImageFiles = _blogImageFileReadRepository.GetWhere(bif => bif.BlogId == Guid.Parse(id));
                    if (!blogImageFiles.IsNullOrEmpty())
                        await blogImageFiles.ForEachAsync(bif => _storageService.DeleteAsync("resources", bif.FileName));

                    await _blogWriteRepository.SaveAsync();

                    return Ok(new { Message = "Silme işlemi başarı ile gerçekleşmiştir." });

                    //frontend tarafinda data null olarak karşılanır. (next ile yakalanan data)
                    //return Ok();
                }
            }

            return BadRequest("Silme aşamasında bir sorun ile karşılaşıldı..");
        }

        [HttpPut]
        public async Task<IActionResult> Put(BlogUpdateModel blogUpdateModel)
        {
            if (ModelState.IsValid)
            {
                Blog blog = await _blogReadRepository.GetByIdAsync(blogUpdateModel.Id);
                blog.Title = blogUpdateModel.Title.Trim();
                blog.CardContext = blogUpdateModel.CardContext;
                blog.Context = blogUpdateModel.Context;

                await _blogWriteRepository.SaveAsync();

                return Ok(blog);
            }

            return BadRequest("Blog güncellenirken bir hata ile karşılaşıldı ...");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadBlogImageForBlogCard(string id)
        {
            var blogImageFiles = _blogImageFileReadRepository.GetWhere(bif => bif.BlogId == Guid.Parse(id) && bif.IsBlogCardImage);

            if (!blogImageFiles.IsNullOrEmpty())
            {
                await blogImageFiles.ForEachAsync(bif => _blogImageFileWriteRepository.Remove(bif));
                await blogImageFiles.ForEachAsync(bif => _storageService.DeleteAsync("resources", bif.FileName));
            }

            var result = await _storageService.UploadAsync("resources", Request.Form.Files);

            Blog blog = await _blogReadRepository.GetByIdAsync(id);
            result.ForEach(r =>
            {
                blog.BlogImageFiles.Add(new BlogImageFile()
                {
                    FileName = r.fileName,
                    Path = r.pathOrContainerNameIncludeFileName,
                    Storage = _storageService.StorageName,
                    IsBlogCardImage = true
                });
            });

            await _blogWriteRepository.SaveAsync();

            return Ok();
        }

        [HttpPost("[action]")] //İlgili bloğa ait resimleri yüklemek için bu action ı kullanıyoruz.
        public async Task<IActionResult> Upload(string id)
        {
            var result = await _storageService.UploadAsync("resources", Request.Form.Files);

            Blog blog = await _blogReadRepository.GetByIdAsync(id);

            await _blogImageFileWriteRepository.AddRangeAsync(result.Select(r => new BlogImageFile()
            {
                FileName = r.fileName,
                Path = r.pathOrContainerNameIncludeFileName,
                Storage = _storageService.StorageName,
                Blog = blog
            }).ToList());

            await _blogImageFileWriteRepository.SaveAsync();

            //2.Yol
            //var result = await _storageService.UploadAsync("blog-images", Request.Form.Files);

            //Blog blog = await _blogReadRepository.GetByIdAsync(id);
            //result.ForEach(r =>
            //{
            //    blog.BlogImageFiles.Add(new BlogImageFile()
            //    {
            //        FileName = r.fileName,
            //        Path = r.pathOrContainerNameIncludeFileName,
            //        Storage = _storageService.StorageName,
            //        IsBlogCardImage = true,
            //        Blog = blog
            //    });
            //});

            //await _blogWriteRepository.SaveAsync();

            return Ok();
        }
    }
}
