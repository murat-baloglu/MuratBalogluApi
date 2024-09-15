using Microsoft.Extensions.Configuration;
using MuratBaloglu.Application.Abstractions.Services;
using System.Net;
using System.Net.Mail;

namespace MuratBaloglu.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMailAsync(string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(Array.Empty<string>(), subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[]? tos, string subject, string body, bool isBodyHtml = true)
        {
            using (MailMessage mailMessage = new MailMessage()) //Maili yada mail mesajı oluşturuyoruz.
            {
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;
                if (tos != null && tos.Length > 0)
                {
                    foreach (var to in tos)
                        mailMessage.To.Add(to);
                }
                else
                {
                    mailMessage.To.Add(_config["Mail:To"]);
                }
                mailMessage.From = new MailAddress(_config["Mail:From"], "Op. Dr. Murat Baloğlu", System.Text.Encoding.UTF8); //Göndericinin bilgileri - Credentials daki Username ile aynı olmalıdır. 

                using (SmtpClient smtp = new SmtpClient()) //Maili gönderiyoruz. Mail sunucusu ile ilgili tüm bilgiler burada işleniyor.
                {
                    smtp.Credentials = new NetworkCredential(_config["Mail:Username"], _config["Mail:Password"]);
                    smtp.Port = Convert.ToInt32(_config["Mail:Port"]);
                    smtp.EnableSsl = true;
                    smtp.Host = _config["Mail:Host"];

                    await smtp.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
