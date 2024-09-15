using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Models;
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
        private readonly IMailService _mailService;

        public ContactsController(IContactReadRepository contactReadRepository, IContactWriteRepository contactWriteRepository, IMailService emailService)
        {
            _contactReadRepository = contactReadRepository;
            _contactWriteRepository = contactWriteRepository;
            _mailService = emailService;
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

            return BadRequest(new { Message = "İletişim bilgileri getirilemiyor." });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Contacts, ActionType = ActionType.Writing, Definition = "İletişim Ekleme veya Güncelleme")]
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

            return BadRequest(new { Message = "İletişim bilgileri eklenirken bir hata ile karşılaşıldı." });
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmailAsync(MailModel mail)
        {
            mail.Phone = mail.Phone != "" ? mail.Phone : "Telefon numarası girilmemiş";

            string body;
            body = "<strong>Mesajı gönderen:</strong>" + "<br>";
            body += "Ad Soyad: " + mail.FullName + "<br>";
            body += "E-posta: " + mail.Email + "<br>";
            body += "Telefon: " + mail.Phone + "<br>";
            body += "Tarih: " + DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss") + "<br>";
            body += $"<p>{mail.Message}</p>" + "<br>";

            mail.Subject = "Murat Baloğlu Web Sitesi İletişim Formundan Gelen Mesajınız Var.";

            await _mailService.SendMailAsync(mail.Subject, body);

            return Ok();
        }
    }
}
