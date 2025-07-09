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

            // ✅ Authenticated landlord: get their own profile
            public const string Me = "/api/landlord/me";

            // ✅ Authenticated landlord: update their own profile
            public const string UpdateMe = "/api/landlord/me";

            // ✅ Authenticated landlord: get their own properties
            public const string GetProperties = "/api/landlord/me/properties";

            // If you want, you can add:
            // public const string ChangePassword = "/api/landlords/me/password";
            // public const string UploadProfileImage = "/api/landlords/me/photo";
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
            public const string Get = "/api/unit";
        }

        public static class Payments
        {
            public const string Pay = "/api/payments";
        }
    }
}
