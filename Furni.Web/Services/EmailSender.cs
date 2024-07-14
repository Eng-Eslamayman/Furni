using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Furni.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly STMPSetting _smtpSetting;

        public EmailSender(IOptions<STMPSetting> smtpSetting, IWebHostEnvironment webHostEnvironment)
        {
            _smtpSetting = smtpSetting.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSetting.Email!, _smtpSetting.Display),
                Body = htmlMessage,
                Subject = subject,
                IsBodyHtml = true
            };

            message.To.Add(email);

            SmtpClient smtpClient = new(_smtpSetting.Host)
            {
                Port = _smtpSetting.Port,
                Credentials = new NetworkCredential(_smtpSetting.Email, _smtpSetting.Password),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);

            smtpClient.Dispose();
        }
    }
}
