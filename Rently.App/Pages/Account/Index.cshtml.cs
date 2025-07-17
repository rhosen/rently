using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using Rently.App.Helpers;
using Rently.Shared.Dtos.Data;
using System.Text.Json;

namespace Rently.App.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public List<PropertyDto> Properties { get; set; }
        public List<UnitDto> Units { get; set; }
        public PropertyDto Property { get; set; }
        public UnitDto Unit { get; set; }

        public bool IsEditMode = false;

        public async Task<IActionResult> OnGetAsync()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var token = User.FindFirst("JWToken")?.Value;
            HttpClient client = GetClient(token);
            await LoadPropertyAsync(client);
            await LoadUnitAsync(client);

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

                Properties = data;
            }
        }

        private async Task LoadUnitAsync(HttpClient client)
        {
            var response = await client.GetAsync(ApiConfig.Unit.Get);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<UnitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<UnitDto>();

                Units = data;
            }
        }
    }
}
