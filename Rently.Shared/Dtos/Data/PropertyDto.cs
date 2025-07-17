namespace Rently.Shared.Dtos.Data
{
    public class PropertyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public int UnitCount { get; set; }
        public decimal PendingPayments { get; set; }
    }
}
