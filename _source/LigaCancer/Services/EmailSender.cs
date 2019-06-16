using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LigaCancer.Services
{
    public class EmailSender : IEmailSender
    {
        // Our private configuration variables
        private readonly string host;
        private readonly int port;
        private readonly bool enableSSL;
        private readonly string userName;
        private readonly string password;
        private readonly string emailFrom;

        // Get our parameterized configuration
        public EmailSender(string host, int port, bool enableSSL, string userName, string password, string emailFrom)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.emailFrom = emailFrom;
            this.password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = enableSSL
            };
            return client.SendMailAsync(
                new MailMessage(emailFrom, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }

    }
}
