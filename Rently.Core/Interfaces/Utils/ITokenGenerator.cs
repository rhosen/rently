using System.Security.Claims;

namespace Rently.Core.Interfaces.Utils
{
    public interface ITokenGenerator
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
