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

            var client = new SmtpClient()
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(
                new MailMesseage()
                );
        }
    }
}
