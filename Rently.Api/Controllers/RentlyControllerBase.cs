using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Rently.Api.Controllers
{
    [ApiController]
    public class RentlyControllerBase : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            // NOTE: 'sub' is automatically mapped to ClaimTypes.NameIdentifier by JwtBearer middleware.
            // So in the API, use User.FindFirst(ClaimTypes.NameIdentifier) to get this ID.
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
        }
    }
}
