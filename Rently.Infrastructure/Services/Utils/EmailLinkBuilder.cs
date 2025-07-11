using Microsoft.Extensions.Options;
using Rently.Core.Interfaces.Utils;
using Rently.Shared.Options;

namespace Rently.Infrastructure.Services.Utils
{
    public class EmailLinkBuilder : IEmailLinkBuilder
    {
        private readonly FrontendOptions _frontendOptions;

        public EmailLinkBuilder(IOptions<FrontendOptions> options)
        {
            _frontendOptions = options.Value;
        }

        public string BuildEmailConfirmationLink(string userId, string token)
        {
            return $"{_frontendOptions.BaseUrl}/Account/ConfirmEmail?userId={userId}&token={token}";
        }
    }
}
