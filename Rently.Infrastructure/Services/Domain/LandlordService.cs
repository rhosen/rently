using Microsoft.EntityFrameworkCore;
using Rently.Core.Entities;
using Rently.Core.Interfaces.Domain;
using Rently.Infrastructure.Data;
using Rently.Shared.Dtos.Auth;

namespace Rently.Infrastructure.Services.Domain
{
    public class LandlordService : ILandlordService
    {
        private readonly RentlyDbContext _context;

        public LandlordService(RentlyDbContext context)
        {
            _context = context;
        }

        public async Task<Landlord> CreateAsync(RegisterDto dto, string identityUserId)
        {
            var landlord = new Landlord
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                StateOrProvince = dto.StateOrProvince,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                IdentityUserId = identityUserId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Landlords.Add(landlord);
            await _context.SaveChangesAsync();
            return landlord;
        }

        public async Task<Landlord?> GetByIdentityUserIdAsync(string identityUserId)
        {
            return await _context.Landlords
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.IdentityUserId == identityUserId);
        }
    }
}
