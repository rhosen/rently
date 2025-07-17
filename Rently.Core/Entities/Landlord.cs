using Microsoft.AspNetCore.Identity;

namespace Rently.Core.Entities
{
    public class Landlord
    {
        public Guid Id { get; set; }

        // Personal Info
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        // Contact Info
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Address Info
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateOrProvince { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Account Meta
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Identity linkage
        public string IdentityUserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        // Navigation
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
