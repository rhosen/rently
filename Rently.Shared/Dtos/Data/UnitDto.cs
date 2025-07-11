namespace Rently.Shared.Dtos.Data
{
    public class UnitDto
    {
        public Guid PropertyId { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public Guid UnitId { get; set; }
        public string UnitNumber { get; set; } = string.Empty;
        public decimal RentAmount { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
