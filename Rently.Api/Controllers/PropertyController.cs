using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rently.Api.Data;
using Rently.Api.Data.Entities;

namespace Rently.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // restrict to logged-in users only
    public class PropertyController : RentlyControllerBase
    {
        private readonly RentlyDbContext _context;

        public PropertyController(RentlyDbContext context)
        {
            _context = context;
        }

        // GET: api/property
        [HttpGet]
        public async Task<IActionResult> GetProperties()
        {
            var landlordId = GetCurrentUserId(); // implement method to get current user (landlord) id

            var properties = await _context.Properties
                .Where(p => p.LandlordId == landlordId && !p.IsDeleted)
                .Select(p => new Common.Dtos.Data.PropertyDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    IsActive = p.IsActive,
                    UnitCount = p.Units.Count(u => !u.IsDeleted),
                })
                .ToListAsync();

            return Ok(properties);
        }

        // POST: api/property
        [HttpPost]
        public async Task<IActionResult> AddProperty([FromBody] Property property)
        {
            var landlordId = GetCurrentUserId();
            property.LandlordId = landlordId;
            property.Id = Guid.NewGuid();
            property.CreatedAt = DateTime.UtcNow;
            property.IsDeleted = false;
            property.IsActive = true;

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProperties), new { id = property.Id }, property);
        }

        // PUT: api/property/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] Property updatedProperty)
        {
            var landlordId = GetCurrentUserId();

            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == id && p.LandlordId == landlordId && !p.IsDeleted);

            if (property == null)
                return NotFound();

            property.Name = updatedProperty.Name;
            property.Address = updatedProperty.Address;
            // don't update Id, LandlordId, CreatedAt

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE (soft delete) api/property/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var landlordId = GetCurrentUserId();

            var property = await _context.Properties
                .Include(p => p.Units.Where(u => !u.IsDeleted && u.IsActive)) // active units only
                .FirstOrDefaultAsync(p => p.Id == id && p.LandlordId == landlordId && !p.IsDeleted);

            if (property == null)
                return NotFound();

            if (property.Units.Any())
                return BadRequest("Cannot delete property with active units.");

            property.IsDeleted = true;
            property.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/property/{id}/deactivate
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateProperty(Guid id)
        {
            var landlordId = GetCurrentUserId();

            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == id && p.LandlordId == landlordId && !p.IsDeleted);

            if (property == null)
                return NotFound();

            property.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/property/{id}/activate
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateProperty(Guid id)
        {
            var landlordId = GetCurrentUserId();

            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == id && p.LandlordId == landlordId && !p.IsDeleted);

            if (property == null)
                return NotFound();

            property.IsActive = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
