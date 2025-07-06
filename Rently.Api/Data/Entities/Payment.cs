namespace Rently.Api.Data.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid UnitId { get; set; }

        public decimal Amount { get; set; }

        // Stripe's unique payment intent or session ID
        public string StripePaymentId { get; set; } = string.Empty;

        // e.g., Pending, Paid, Failed, Refunded, etc.
        public string Status { get; set; } = "Pending";

        // When tenant paid (nullable for pending)
        public DateTime? PaymentDate { get; set; }

        // For monthly rent tracking
        public int Month { get; set; }  // 1 to 12
        public int Year { get; set; }   // e.g., 2025

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Unit? Unit { get; set; }

        // Optional: virtual PaymentCode for referencing
        public string PaymentCode => $"{UnitId.ToString().Substring(0, 6)}-{Year}{Month:D2}";
    }
}
