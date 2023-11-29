using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EfCore.service.impl
{
    public class VerificationCodeManager
    {
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private static readonly Random _random = new Random();

        private readonly IConfiguration _configuration;

        public VerificationCodeManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void StoreVerificationCode(string email, string code)
        {
            // Додати код в тимчасове сховище з ключем, який асоціюється з електронною адресою
            _cache.Set(email, code, DateTimeOffset.Now.AddMinutes(30)); // Наприклад, тут код діє 30 хвилин
        }

        public static bool VerifyCode(string email, string enteredCode)
        {
            // Отримати код з тимчасового сховища за електронною адресою
            if (_cache.TryGetValue(email, out string storedCode))
            {
                // Порівняти введений код із збереженим
                return string.Equals(storedCode, enteredCode, StringComparison.OrdinalIgnoreCase);
            }

            return false; // Код не знайдено або прострочено
        }

        public static string GenerateVerificationCode()
        {
            int code = _random.Next(10000, 100000);
            return code.ToString();
        }

        public static void SendEmail(string to, string subject, string body)
        {
            /*var smtpClient = new SmtpClient("smtp.google.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("impact.project.lnu@gmail.com", "dreamteamlegends"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("impact.project.lnu@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            smtpClient.Send(mailMessage);*/

            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(MailboxAddress.Parse("impact.project.lnu@gmail.com"));
            mailMessage.To.Add(MailboxAddress.Parse(to));


            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };


            SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                smtpClient.Authenticate("impact.project.lnu@gmail.com", "babv hjjc uqom newk");
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                smtpClient.Disconnect(true);
                smtpClient.Dispose();

            }
        }
    }
}
