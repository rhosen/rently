using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using Rently.App.Models;
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
        public UnitResponse Unit { get; set; }
        public KpiModel Kpi { get; set; }

        public bool IsEditMode = false;

        public async Task<IActionResult> OnGetAsync()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            Email = User?.Identity?.Name ?? "Landlord";
            var token = User.FindFirst("JWToken")?.Value;
            HttpClient client = GetClient(token);
            await LoadPropertyAsync(client);
            await LoadUnitAsync(client);

            return Page();
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
                Kpi = new KpiModel
                {
                    TotalProperties = Properties.Count,
                    TotalUnits = Properties.Sum(x => x.UnitCount)
                };
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
