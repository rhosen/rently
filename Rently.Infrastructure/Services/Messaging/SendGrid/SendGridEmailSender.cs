using Microsoft.Extensions.Options;
using Rently.Core.Interfaces.Messaging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Rently.Infrastructure.Services.Messaging.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions _options;

        public SendGridEmailSender(IOptions<SendGridOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var client = new SendGridClient(_options.ApiKey);
            var from = new EmailAddress(_options.SenderEmail, _options.SenderName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, message);
            await client.SendEmailAsync(msg);
        }
    }
}
