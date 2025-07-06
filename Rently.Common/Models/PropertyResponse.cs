namespace Rently.Common.Models
{
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public int UnitCount { get; set; }
        public decimal PendingPayments { get; set; }
    }
}
