using Rently.Api.Helper;
using Rently.Core.Interfaces.Account;
using Rently.Core.Interfaces.Domain;
using Rently.Core.Interfaces.Messaging;
using Rently.Core.Interfaces.Utils;
using Rently.Infrastructure.Services.Account;
using Rently.Infrastructure.Services.Domain;
using Rently.Infrastructure.Services.Messaging.Mailgun;
using Rently.Infrastructure.Services.Utils;
using Rently.Shared.Options;

namespace Rently.API.Extensions;

public static class RentlyServiceExtensions
{
    public static IServiceCollection ConfigureRentlyAppServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<FrontendOptions>(config.GetSection("FrontendOptions"));
        services.AddScoped<IEmailLinkBuilder, EmailLinkBuilder>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILandlordService, LandlordService>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();


        services.Configure<MailgunOptions>(options =>
        {
            options.ApiKey = Environment.GetEnvironmentVariable("Mailgun__ApiKey");
            options.Domain = Environment.GetEnvironmentVariable("Mailgun__Domain");
            options.From = Environment.GetEnvironmentVariable("Mailgun__FromEmail");
        });

        services.AddHttpClient("MailgunClient", client =>
        {
            client.BaseAddress = new Uri(config["Mailgun:BaseUrl"]);
        });

        services.AddTransient<IEmailSender, MailgunEmailSender>();

        return services;
    }
}
