using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Models.Contact;
using MuratBaloglu.Application.Repositories.ContactRepository;
using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactReadRepository _contactReadRepository;
        private readonly IContactWriteRepository _contactWriteRepository;

        public ContactsController(IContactReadRepository contactReadRepository, IContactWriteRepository contactWriteRepository)
        {
            _contactReadRepository = contactReadRepository;
            _contactWriteRepository = contactWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var contact = await _contactReadRepository.GetAll(false).Select(c => new
                {
                    c.Address,
                    c.Email,
                    c.FixedPhoneOne,
                    c.FixedPhoneOneExtension,
                    c.FixedPhoneTwo,
                    c.FixedPhoneTwoExtension,
                    c.Mobile,
                    c.GoogleMap
                }).FirstOrDefaultAsync();

                return Ok(contact);
            }

            return BadRequest("İletişim bilgileri getirilemiyor ...");
        }

        [HttpPost]
        public async Task<IActionResult> Post(ContactAddModel contactAddModel)
        {
            if (ModelState.IsValid)
            {
                var query = await _contactReadRepository.GetAll(false).FirstOrDefaultAsync();

                if (query is null)
                {
                    Contact contact = new Contact()
                    {
                        Address = contactAddModel.Address,
                        Email = contactAddModel.Email,
                        FixedPhoneOne = contactAddModel.FixedPhoneOne,
                        FixedPhoneOneExtension = contactAddModel.FixedPhoneOneExtension,
                        FixedPhoneTwo = contactAddModel.FixedPhoneTwo,
                        FixedPhoneTwoExtension = contactAddModel.FixedPhoneTwoExtension,
                        Mobile = contactAddModel.Mobile,
                        GoogleMap = contactAddModel.GoogleMap
                    };

                    await _contactWriteRepository.AddAsync(contact);
                    await _contactWriteRepository.SaveAsync();
                    return Ok(contactAddModel);
                }

                query.Address = contactAddModel.Address;
                query.Email = contactAddModel.Email;
                query.FixedPhoneOne = contactAddModel.FixedPhoneOne;
                query.FixedPhoneOneExtension = contactAddModel.FixedPhoneOneExtension;
                query.FixedPhoneTwo = contactAddModel.FixedPhoneTwo;
                query.FixedPhoneTwoExtension = contactAddModel.FixedPhoneTwoExtension;
                query.Mobile = contactAddModel.Mobile;
                query.GoogleMap = contactAddModel.GoogleMap;

                _contactWriteRepository.Update(query);
                await _contactWriteRepository.SaveAsync();
                return Ok(contactAddModel);

            }

            return BadRequest("İletişim bilgileri eklenirken bir hata ile karşılaşıldı ...");
        }
    }
}
