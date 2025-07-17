using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rently.Core.Interfaces.Account;
using Rently.Core.Interfaces.Domain;
using Rently.Infrastructure.Data;
using Rently.Shared.Dtos.Auth;
using Rently.Shared.Dtos.Data;

namespace Rently.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // restrict to logged-in users only
    public class AccountController : RentlyControllerBase
    {
        private readonly RentlyDbContext _context;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        public AccountController(RentlyDbContext context,
                                  IAuthService authService,
                                  IAccountService accountService)
        {
            _context = context;
            _authService = authService;
            _accountService = accountService;
        }

        // ✅ GET /api/account/me
        [HttpGet("me")]
        public async Task<ActionResult<AccountDto>> GetProfile()
        {
            var accountId = GetCurrentUserId();

            var account = await _accountService.GetAccountAsync(accountId);

            if (account == null)
                return NotFound();

            var dto = new AccountDto
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                DateOfBirth = account.DateOfBirth,
                PhoneNumber = account.PhoneNumber,
                StreetAddress = account.StreetAddress,
                City = account.City,
                StateOrProvince = account.StateOrProvince,
                PostalCode = account.PostalCode,
                Country = account.Country,
                Email = account.Email
            };

            return Ok(dto);
        }

        // ✅ PUT /api/account/me
        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] AccountDto dto)
        {

            var accountId = GetCurrentUserId();

            var account = await _accountService.GetAccountAsync(accountId);

            if (account == null)
                return NotFound();

            // ✅ Update allowed fields only
            account.FirstName = dto.FirstName;
            account.LastName = dto.LastName;
            account.DateOfBirth = dto.DateOfBirth;
            account.PhoneNumber = dto.PhoneNumber;
            account.StreetAddress = dto.StreetAddress;
            account.City = dto.City;
            account.StateOrProvince = dto.StateOrProvince;
            account.PostalCode = dto.PostalCode;
            account.Country = dto.Country;

            account.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ POST /api/account/me/change-password
        [HttpPost("me/change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var accountId = GetCurrentUserId();

            if (accountId == Guid.Empty) return Unauthorized();

            var result = await _authService.ChangePassword(dto, accountId);

            if (result.IsSuccess) return Ok(result.Data);

            return StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
    }
}
