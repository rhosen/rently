using Rently.Core.Entities;
using Rently.Shared.Dtos.Auth;
using Rently.Shared.Dtos.Data;

namespace Rently.Core.Interfaces.Domain
{
    public interface IAccountService
    {
        Task<Entities.Account> CreateAsync(RegisterDto dto, string identityId);
        Task<Entities.Account?> GetAccountByIdentityAsync(string identityId);
        Task<Entities.Account?> GetAccountAsync(Guid accountId);
    }
}
