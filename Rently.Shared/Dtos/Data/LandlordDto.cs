namespace Rently.Shared.Dtos.Data
{
    public class LandlordDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string StreetAddress { get; set; } = "";
        public string City { get; set; } = "";
        public string StateOrProvince { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string Country { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
