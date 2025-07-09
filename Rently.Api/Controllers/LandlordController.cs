using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rently.Api.Data;
using Rently.Api.Interfaces;
using Rently.Common.Dtos.Auth;
using Rently.Common.Dtos.Data;

namespace Rently.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // restrict to logged-in users only
    public class LandlordController: RentlyControllerBase
    {
        private readonly RentlyDbContext _context;
        private readonly IAccountService _accountService;
        public LandlordController(RentlyDbContext context,
                                  IAccountService accountService)
        {
            _context = context;
            _accountService =  accountService;
        }

        // ✅ GET /api/landlord/me
        [HttpGet("me")]
        public async Task<ActionResult<LandlordDto>> GetProfile()
        {
            Data.Entities.Landlord? landlord = await GetLandlord();

            if (landlord == null)
                return NotFound();

            var dto = new LandlordDto
            {
                FirstName = landlord.FirstName,
                LastName = landlord.LastName,
                DateOfBirth = landlord.DateOfBirth,
                PhoneNumber = landlord.PhoneNumber,
                StreetAddress = landlord.StreetAddress,
                City = landlord.City,
                StateOrProvince = landlord.StateOrProvince,
                PostalCode = landlord.PostalCode,
                Country = landlord.Country,
                Email = landlord.Email
            };

            return Ok(dto);
        }

        // ✅ PUT /api/landlord/me
        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] LandlordDto dto)
        {
            Data.Entities.Landlord? landlord = await GetLandlord();

            if (landlord == null)
                return NotFound();

            // ✅ Update allowed fields only
            landlord.FirstName = dto.FirstName;
            landlord.LastName = dto.LastName;
            landlord.DateOfBirth = dto.DateOfBirth;
            landlord.PhoneNumber = dto.PhoneNumber;
            landlord.StreetAddress = dto.StreetAddress;
            landlord.City = dto.City;
            landlord.StateOrProvince = dto.StateOrProvince;
            landlord.PostalCode = dto.PostalCode;
            landlord.Country = dto.Country;

            landlord.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<Data.Entities.Landlord?> GetLandlord()
        {
            var landlordId = GetCurrentUserId();

            var landlord = await _context.Landlords.FirstOrDefaultAsync(l => l.Id == landlordId);
            return landlord;
        }

        // ✅ POST /api/landlord/me/change-password
        [HttpPost("me/change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var landlordId = GetCurrentUserId();

            if (landlordId == null) return Unauthorized();

            var landlord = await _context.Landlords.FirstOrDefaultAsync(x => x.Id == landlordId);

            var result = await _accountService.ChangePassword(dto, landlord.IdentityUserId);
            
            if (!result) return BadRequest(new { Message = "Failed to change Password." });

            return Ok(new { Message = "Password changed successfully." });
        }
    }
}
