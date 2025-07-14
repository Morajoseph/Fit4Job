using System.Net;
using System.Net.Mail;
using Fit4Job.Services.Interfaces;

namespace Fit4Job.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string receptor, string subject, string body)
        {
            var email = _configuration.GetValue<string>("EmailSettingsConfiguration:Email");
            var password = _configuration.GetValue<string>("EmailSettingsConfiguration:Password");
            var host = _configuration.GetValue<string>("EmailSettingsConfiguration:Host");
            var port = _configuration.GetValue<int>("EmailSettingsConfiguration:Port");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email!, receptor, subject, body);
            await smtpClient.SendMailAsync(message);
        }
    }
}
