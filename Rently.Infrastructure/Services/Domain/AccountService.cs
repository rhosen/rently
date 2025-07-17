using Microsoft.EntityFrameworkCore;
using Rently.Core.Interfaces.Domain;
using Rently.Infrastructure.Data;
using Rently.Shared.Dtos.Auth;

namespace Rently.Infrastructure.Services.Domain
{
    public class AccountService : IAccountService
    {
        private readonly RentlyDbContext _context;

        public AccountService(RentlyDbContext context)
        {
            _context = context;
        }

        public async Task<Core.Entities.Account> CreateAsync(RegisterDto dto, string identityUserId)
        {
            var landlord = new Core.Entities.Account
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

            _context.Accounts.Add(landlord);
            await _context.SaveChangesAsync();
            return landlord;
        }

        public async Task<Core.Entities.Account?> GetAccountAsync(Guid accountId)
        {
            return await _context.Accounts
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == accountId);
        }

        public async Task<Core.Entities.Account?> GetAccountByIdentityAsync(string identityUserId)
        {
            return await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
        }

    }
}
