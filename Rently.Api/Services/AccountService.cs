using Microsoft.AspNetCore.Identity;
using Rently.Api.Interfaces;
using Rently.Common.Dtos.Auth;

namespace Rently.Api.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> ChangePassword(ChangePasswordDto dto, string identityId)
        {
            var user = await _userManager.FindByIdAsync(identityId);

            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            return result.Succeeded;

        }
    }
}

