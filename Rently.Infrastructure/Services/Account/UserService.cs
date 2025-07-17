using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Rently.Core.Interfaces.Account;
using Rently.Core.Interfaces.Messaging;
using Rently.Core.Interfaces.Utils;
using Rently.Shared.Logging;
using System.Net;

namespace Rently.Infrastructure.Services.Account
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailLinkBuilder _emailLinkBuilder;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly ILogService _logService;


        public UserService(UserManager<IdentityUser> userManager,
                           IEmailLinkBuilder emailLinkBuilder,
                           IEmailSender emailSender,
                           IWebHostEnvironment env,
                           ILogService logService)
        {
            _userManager = userManager;
            _emailLinkBuilder = emailLinkBuilder;
            _emailSender = emailSender;
            _env = env;
            _logService = logService;
        }

        public async Task<IdentityUser> CreateUserAsync(string email, string password, string phoneNumber)
        {
            var u = new IdentityUser { UserName = email, Email = email, PhoneNumber = phoneNumber };
            var res = await _userManager.CreateAsync(u, password);
            if (!res.Succeeded) throw new Exception(string.Join(", ", res.Errors.Select(e => e.Description)));
            return u;
        }

        public async Task SendConfirmationEmailAsync(IdentityUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var link = _emailLinkBuilder.BuildEmailConfirmationLink(user.Id, encodedToken);
            if (_env.IsDevelopment()) {
                _logService.LogInfo($"[DEV MODE] Email confirmation link for {user.Email}: {link}");
                return;
            }
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{link}'>clicking here</a>.");
        }
    }

}
