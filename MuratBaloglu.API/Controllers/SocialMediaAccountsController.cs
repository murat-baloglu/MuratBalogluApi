using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Models.Contact;
using MuratBaloglu.Application.Models.SocialMediaAccounts;
using MuratBaloglu.Application.Repositories.SocialMediaAccountRepository;
using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaAccountsController : ControllerBase
    {
        private readonly ISocialMediaAccountReadRepository _socialMediaAccountReadRepository;
        private readonly ISocialMediaAccountWriteRepository _socialMediaAccountWriteRepository;

        public SocialMediaAccountsController(ISocialMediaAccountReadRepository socialMediaAccountReadRepository, ISocialMediaAccountWriteRepository socialMediaAccountWriteRepository)
        {
            _socialMediaAccountReadRepository = socialMediaAccountReadRepository;
            _socialMediaAccountWriteRepository = socialMediaAccountWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var socialMediaAccount = await _socialMediaAccountReadRepository.GetAll(false).Select(s => new
                {
                    s.YouTube,
                    s.LinkedIn,
                    s.Instagram,
                    s.X,
                    s.Facebook
                }).FirstOrDefaultAsync();

                return Ok(socialMediaAccount);
            }

            return BadRequest("Sosyal medya hesapları getirilemiyor ...");
        }

        [HttpPost]
        public async Task<IActionResult> Post(SocialMediaAccountAddModel socialMediaAccountAddModel)
        {
            if (ModelState.IsValid)
            {
                var query = await _socialMediaAccountReadRepository.GetAll(false).FirstOrDefaultAsync();

                if (query is null)
                {
                    SocialMediaAccount socialMediaAccount = new SocialMediaAccount()
                    {
                        YouTube = socialMediaAccountAddModel.YouTube,
                        Facebook = socialMediaAccountAddModel.Facebook,
                        Instagram = socialMediaAccountAddModel.Instagram,
                        LinkedIn = socialMediaAccountAddModel.LinkedIn,
                        X = socialMediaAccountAddModel.X
                    };

                    await _socialMediaAccountWriteRepository.AddAsync(socialMediaAccount);
                    await _socialMediaAccountWriteRepository.SaveAsync();
                    return Ok(socialMediaAccountAddModel);
                }

                query.YouTube = socialMediaAccountAddModel.YouTube;
                query.Facebook = socialMediaAccountAddModel.Facebook;
                query.Instagram = socialMediaAccountAddModel.Instagram;
                query.LinkedIn = socialMediaAccountAddModel.LinkedIn;
                query.X = socialMediaAccountAddModel.X;

                _socialMediaAccountWriteRepository.Update(query);
                await _socialMediaAccountWriteRepository.SaveAsync();
                return Ok(socialMediaAccountAddModel);

            }

            return BadRequest("Sosyal medya hesapları eklenirken bir hata ile karşılaşıldı ...");
        }
    }
}
