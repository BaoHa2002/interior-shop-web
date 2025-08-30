using InteriorShop.Application.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

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
            var host = _config["Smtp:Host"];
            if (string.IsNullOrEmpty(host))
            {
                _logger.LogWarning("Smtp not configured. Skipping email to {to}", toEmail);
                return;
            }

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config["Smtp:FromEmail"] ?? "no-reply@example.com"));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(host, int.Parse(_config["Smtp:Port"] ?? "587"), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
