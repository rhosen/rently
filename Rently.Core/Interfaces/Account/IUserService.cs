using Microsoft.AspNetCore.Identity;
using Rently.Shared.Dtos.Auth;

namespace Rently.Core.Interfaces.Account
{
    public interface IUserService
    {
        Task<IdentityUser> CreateUserAsync(string email, string password, string phoneNumber);
        Task SendConfirmationEmailAsync(IdentityUser user);
    }
}
