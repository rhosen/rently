using Rently.Common.Dtos.Auth;

namespace Rently.Api.Interfaces
{
    public interface IAccountService
    {
        Task<bool> ChangePassword(ChangePasswordDto dto, string identityId);
    }
}
