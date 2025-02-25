using BadmintonSocialNetwork.Service.DTOs;
using BadmintonSocialNetwork.Service.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.Services
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(MailContent mailContent);
    }
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendMailAsync(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail); //new MailboxAddress("Tên", "Email")
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = mailContent.Body;

            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                DotNetEnv.Env.Load();

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                var password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
                smtp.Authenticate(_mailSettings.Mail, password);
                smtp.Send(email);

                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
    public class MailContent
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
