using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using Rently.App.Helpers;
using Rently.App.Models;
using Rently.Common.Dtos.Data;
using System.Text.Json;

namespace Rently.App.Pages.Landlord
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public KpiModel Kpi { get; set; }
        public string Email { get; private set; }

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

            return Page();
        }

        private HttpClient GetClient(string token)
        {
            var client = _httpClientFactory.CreateClient("RentlyApi");
            client.AddBearerToken(token);
            return client;
        }

        private async Task LoadPropertyAsync(HttpClient client)
        {
            var response = await client.GetAsync(ApiConfig.Property.Get);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<PropertyDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<PropertyDto>();

                Kpi = new KpiModel
                {
                    TotalProperties = data.Count,
                    TotalUnits = data.Sum(x => x.UnitCount)
                };
            }
        }

    }
}
