using Rently.Core.Entities;
using Rently.Shared.Dtos.Auth;

namespace Rently.Core.Interfaces.Domain
{
    public interface ILandlordService
    {
        Task<Landlord> CreateAsync(RegisterDto dto, string identityUserId);
        Task<Landlord?> GetByIdentityUserIdAsync(string userId);
    }
}
