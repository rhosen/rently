namespace Rently.App.Configs
{
    public static class ApiConfig
    {
        public const string BaseUrl = "https://localhost:7164";

        public static class Auth
        {
            public const string Register = "/api/auth/register";
            public const string Login = "/api/auth/login";
        }

        public static class Landlord
        {
            public const string Create = "/api/landlords";
            public const string Get = "/api/landlords/{id}";
            public const string GetProperties = "/api/landlords/me/properties";

            // Add more as you build
        }

        public static class Property
        {
            public const string Create = "/api/properties";
            public const string GetByLandlord = "/api/landlords/{landlordId}/properties";
            public const string Get = "/api/property";
        }

        public static class Unit
        {
            public const string Create = "/api/units";
        }

        public static class Payments
        {
            public const string Pay = "/api/payments";
        }
    }
}
