using Rently.Shared.Dtos.Auth;

namespace Rently.Core.Interfaces.Account
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<string> ResendConfirmationEmailAsync(ResendEmailDto dto);
        Task<bool> ChangePassword(ChangePasswordDto dto, string identityId);
        Task<ResultDto<AuthDto>> AuthenticateUser(LoginDto loginDto);
    }
}
