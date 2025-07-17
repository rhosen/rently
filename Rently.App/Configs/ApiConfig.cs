namespace Rently.App.Configs
{
    public static class ApiConfig
    {
        public const string BaseUrl = "https://localhost:7164";

        public static class Auth
        {
            public const string Register = "/api/auth/register";
            public const string Login = "/api/auth/login";
            public const string ConfirmEmail = "api/auth/confirm-email?userId=";
            public const string ReconfirmEmail = "/api/auth/resend-email-confirmation";
        }

        public static class Account
        {
            public const string Create = "/api/accounts";

            // ✅ Authenticated account: get their own profile
            public const string Me = "/api/account/me";

            // ✅ Authenticated account: update their own profile
            public const string UpdateMe = "/api/account/me";

            // ✅ Authenticated account: get their own properties
            public const string GetProperties = "/api/account/me/properties";

            // If you want, you can add:
            public const string ChangePassword = "/api/account/me/change-password";
            // public const string UploadProfileImage = "/api/accounts/me/photo";
        }


        public static class Property
        {
            public const string Create = "/api/properties";
            public const string GetByAccount = "/api/accounts/{accountId}/properties";
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
