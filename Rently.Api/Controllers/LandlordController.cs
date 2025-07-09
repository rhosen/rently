using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rently.Api.Data;
using Rently.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Rently.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // restrict to logged-in users only
    public class LandlordController: RentlyControllerBase
    {
        private readonly RentlyDbContext _context;
        public LandlordController(RentlyDbContext context)
        {
            _context = context;
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
    }
}
