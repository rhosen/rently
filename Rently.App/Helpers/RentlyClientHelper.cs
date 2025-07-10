using System.Net.Http.Headers;

namespace Rently.App.Helpers
{
    public static class RentlyClientHelper
    {
        /// <summary>
        /// Adds a Bearer token to the Authorization header.
        /// </summary>
        /// <param name="client">The HttpClient instance.</param>
        /// <param name="token">The token string.</param>
        public static void AddBearerToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
