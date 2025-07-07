using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using Rently.Common.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Rently.App.Pages.Landlord
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public string Email { get; set; } = "Landlord";

        public List<PropertyResponse> Properties { get; set; }
        public List<UnitResponse> Units { get; set; }
        public PropertyResponse Property { get; set; }
        public UnitResponse Unit{ get; set; }

        public bool IsEditMode = false;

        public int TotalUnits = 0;
        public decimal TotalPendingPayments = 0;

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                Response.Redirect("/Account/Login");
                return;
            }

            Email = User?.Identity?.Name ?? "Landlord";
            HttpClient client = GetClient(token);
            await LoadPropertyAsync(client);
            await LoadUnitAsync(client);
        }

        private HttpClient GetClient(string token)
        {
            var client = _httpClientFactory.CreateClient("RentlyApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private async Task LoadPropertyAsync(HttpClient client)
        {
            var response = await client.GetAsync(ApiConfig.Property.Get);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<PropertyResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<PropertyResponse>();

                Properties = data;
                TotalUnits = Properties.Sum(x => x.UnitCount);
            }
        }

        private async Task LoadUnitAsync(HttpClient client)
        {
            var response = await client.GetAsync(ApiConfig.Unit.Get);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<UnitResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<UnitResponse>();

                Units = data;
            }
        }
    }
}
