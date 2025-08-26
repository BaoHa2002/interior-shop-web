using InteriorShop.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace InteriorShop.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            //Chưa có cấu hình SMTP thì mock log
            if (string.IsNullOrWhiteSpace(_config["Smtp:Host"]))
            {
                _logger.LogWarning("SMTP chưa cấu hình. Email MOCK:");
                _logger.LogInformation("To: {to}", toEmail);
                _logger.LogInformation("Subject: {subject}", subject);
                _logger.LogInformation("Body: {body}", htmlBody);
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PhatDecors", _config["Smtp:FromEmail"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _config["Smtp:Host"],
                int.Parse(_config["Smtp:Port"] ?? "587"),
                MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email sent to {to}", toEmail);
        }
    }
}
