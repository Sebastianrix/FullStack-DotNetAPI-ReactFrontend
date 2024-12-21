using System.Net.Mail;
using System.Net;

namespace WebApi.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message) 
        {
            var mail = "RealEmailAdress@outlook.com";
            var pw = "Password123";

            var client = new SmtpClient("smtp.office365.com",587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(
                new MailMesseage(from: mail,
                ToString: email,
                subject,
                message));
        }
    }
}
