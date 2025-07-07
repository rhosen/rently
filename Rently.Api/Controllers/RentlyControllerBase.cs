using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rently.Api.Controllers
{
    [ApiController]
    public class RentlyControllerBase: ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return Guid.TryParse(userIdString, out var userId)? userId : Guid.Empty;
        }
    }
}
