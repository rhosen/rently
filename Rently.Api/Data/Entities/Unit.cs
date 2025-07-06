namespace Rently.Api.Data.Entities
{
    public class Unit
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }

        public string UnitNumber { get; set; } = string.Empty;
        public decimal RentAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public Property? Property { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
