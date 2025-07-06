﻿namespace Rently.Api.Data.Entities
{
    public class Landlord
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
