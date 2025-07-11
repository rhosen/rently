using Rently.Shared.Dtos.Auth;

namespace Rently.Core.Interfaces.Account
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> ResendConfirmationEmailAsync(ResendEmailDto dto);
        Task<bool> ChangePassword(ChangePasswordDto dto, string identityId);
        Task<AuthDto> AuthenticateUser(LoginDto loginDto);
    }
}
