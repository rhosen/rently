using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using Rently.Common.Dtos.Data;
using System.Net.Http.Headers;

namespace Rently.App.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LandlordDto Landlord { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = User.FindFirst("JWToken")?.Value;

            if (string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("RentlyApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(ApiConfig.Landlord.Me);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to load profile.");
                return Page();
            }

            Landlord = await response.Content.ReadFromJsonAsync<LandlordDto>() ?? new LandlordDto();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = User.FindFirst("JWToken")?.Value;

            if (string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("RentlyApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync(ApiConfig.Landlord.UpdateMe, Landlord);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to update profile.");
                return Page();
            }

            // ✅ If name changed: update cookie claims if you want — optional
            // (Could sign-in again with new claims if needed)

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage();
        }
    }
}
