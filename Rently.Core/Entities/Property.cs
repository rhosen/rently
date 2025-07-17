namespace Rently.Core.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;

        // Active = true means property is available/rentable; false means temporarily disabled
        public bool IsActive { get; set; } = true;

        public Account? Landlord { get; set; }
        public ICollection<Unit> Units { get; set; } = new List<Unit>();
    }

}
