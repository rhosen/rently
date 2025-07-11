using Microsoft.Extensions.Options;
using Rently.Core.Interfaces.Messaging;
using System.Net.Http.Headers;
using System.Text;

namespace Rently.Infrastructure.Services.Messaging.Mailgun
{
    public class MailgunEmailSender : IEmailSender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MailgunOptions _options;

        public MailgunEmailSender(IHttpClientFactory httpClientFactory, IOptions<MailgunOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var client = _httpClientFactory.CreateClient("MailgunClient");

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{_options.ApiKey}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("from", _options.From),
            new KeyValuePair<string, string>("to", toEmail),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("html", htmlMessage)
        });

            var response = await client.PostAsync($"/v3/{_options.Domain}/messages", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Mailgun error: {error}");
            }
        }
    }
}
