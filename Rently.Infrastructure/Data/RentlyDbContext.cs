using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rently.Core.Entities;

namespace Rently.Infrastructure.Data
{
    public class RentlyDbContext : IdentityDbContext<IdentityUser>
    {
        public RentlyDbContext(DbContextOptions<RentlyDbContext> options)
        : base(options)
        {
        }

        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
