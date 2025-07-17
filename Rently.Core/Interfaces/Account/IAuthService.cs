using Rently.Shared.Dtos.Auth;

namespace Rently.Core.Interfaces.Account
{
    public interface IAuthService
    {
        Task<ResultDto<string>> RegisterAsync(RegisterDto dto);
        Task<ResultDto<bool>> ConfirmEmailAsync(string userId, string token);
        Task<ResultDto<string>> ResendConfirmationEmailAsync(ResendEmailDto dto);
        Task<ResultDto<bool>> ChangePassword(ChangePasswordDto dto, Guid accountId);
        Task<ResultDto<AuthDto>> AuthenticateUser(LoginDto loginDto);
    }
}
