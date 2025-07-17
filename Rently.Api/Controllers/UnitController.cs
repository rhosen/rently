using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rently.Core.Entities;
using Rently.Infrastructure.Data;
using Rently.Shared.Dtos.Data;

namespace Rently.Api.Controllers
{
    namespace Rently.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class UnitController : RentlyControllerBase
        {
            private readonly RentlyDbContext _context;

            public UnitController(RentlyDbContext context)
            {
                _context = context;
            }

            // GET: api/unit
            [HttpGet]
            public async Task<IActionResult> GetUnits()
            {
                var landlordId = GetCurrentUserId(); // implement method to get current user (landlord) id
                var units = await (
                                    from unit in _context.Units
                                    join property in _context.Properties on unit.PropertyId equals property.Id
                                    where property.AccountId == landlordId && !unit.IsDeleted && !property.IsDeleted
                                    select new UnitDto
                                    {
                                        UnitId = unit.Id,
                                        PropertyId = unit.PropertyId,
                                        PropertyName = property.Name,
                                        PropertyAddress = property.Address,
                                        UnitNumber = unit.UnitNumber,
                                        RentAmount = unit.RentAmount,
                                        IsActive = unit.IsActive,
                                    }
                                    ).ToListAsync();
                return Ok(units);
            }

            // GET: api/unit/property/{propertyId}
            [HttpGet("property/{propertyId}")]
            public async Task<IActionResult> GetUnitsByProperty(Guid propertyId)
            {
                var landlordId = GetCurrentUserId();

                // Check if property belongs to current landlord and is not deleted
                var property = await _context.Properties
                    .FirstOrDefaultAsync(p => p.Id == propertyId && p.AccountId == landlordId && !p.IsDeleted);

                if (property == null)
                    return NotFound("Property not found or access denied.");

                var units = await _context.Units
                    .Where(u => u.PropertyId == propertyId && !u.IsDeleted)
                    .ToListAsync();

                return Ok(units);
            }

            // POST: api/unit
            [HttpPost]
            public async Task<IActionResult> AddUnit([FromBody] Unit unit)
            {
                var landlordId = GetCurrentUserId();

                var property = await _context.Properties
                    .FirstOrDefaultAsync(p => p.Id == unit.PropertyId && p.AccountId == landlordId && !p.IsDeleted);

                if (property == null)
                    return BadRequest("Invalid property or access denied.");

                unit.Id = Guid.NewGuid();
                unit.CreatedAt = DateTime.UtcNow;
                unit.IsDeleted = false;
                unit.IsActive = true;

                _context.Units.Add(unit);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUnitsByProperty), new { propertyId = unit.PropertyId }, unit);
            }

            // PUT: api/unit/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateUnit(Guid id, [FromBody] Unit updatedUnit)
            {
                var landlordId = GetCurrentUserId();

                var unit = await _context.Units
                    .Include(u => u.Property)
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Property.AccountId == landlordId);

                if (unit == null)
                    return NotFound();

                unit.UnitNumber = updatedUnit.UnitNumber;
                unit.RentAmount = updatedUnit.RentAmount;
                // Don't update PaymentCode, PropertyId, CreatedAt

                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/unit/{id} (soft delete)
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUnit(Guid id)
            {
                var landlordId = GetCurrentUserId();

                var unit = await _context.Units
                    .Include(u => u.Property)
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Property.AccountId == landlordId);

                if (unit == null)
                    return NotFound();

                unit.IsDeleted = true;
                unit.IsActive = false;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            // PATCH: api/unit/{id}/deactivate
            [HttpPatch("{id}/deactivate")]
            public async Task<IActionResult> DeactivateUnit(Guid id)
            {
                var landlordId = GetCurrentUserId();

                var unit = await _context.Units
                    .Include(u => u.Property)
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Property.AccountId == landlordId);

                if (unit == null)
                    return NotFound();

                unit.IsActive = false;
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // PATCH: api/unit/{id}/activate
            [HttpPatch("{id}/activate")]
            public async Task<IActionResult> ActivateUnit(Guid id)
            {
                var landlordId = GetCurrentUserId();

                var unit = await _context.Units
                    .Include(u => u.Property)
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Property.AccountId == landlordId);

                if (unit == null)
                    return NotFound();

                unit.IsActive = true;
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

}
