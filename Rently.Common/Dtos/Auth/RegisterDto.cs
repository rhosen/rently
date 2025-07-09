namespace Rently.Common.Dtos.Auth
{
    public class RegisterDto
    {
        // Identity fields
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Personal Info
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        // Contact Info
        public string PhoneNumber { get; set; } = string.Empty;

        // Address Info
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateOrProvince { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

}
