using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using Rently.Shared.Dtos.Auth;
using Rently.Shared.Dtos.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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
        public AccountDto Account { get; set; } = new();

        [BindProperty]
        public ChangePasswordDto ChangePasswordDto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = User.FindFirst("JWToken")?.Value;

            if (string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("RentlyApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(ApiConfig.Account.Me);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to load profile.");
                return Page();
            }

            Account = await response.Content.ReadFromJsonAsync<AccountDto>() ?? new AccountDto();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            var token = User.FindFirst("JWToken")?.Value;

            if (string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("RentlyApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync(ApiConfig.Account.UpdateMe, Account);

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

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid) return Page();

            var token = User.Claims.FirstOrDefault(c => c.Type == "JWToken")?.Value;

            var client = _httpClientFactory.CreateClient("RentlyApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(ChangePasswordDto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiConfig.Account.ChangePassword, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Password change failed: {errorBody}");
                return Page();
            }

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToPage();
        }

    }
}
