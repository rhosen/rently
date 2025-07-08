using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using System.Text;
using System.Text.Json;

namespace Rently.App.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }


        public IActionResult OnGet()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Landlord/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("RentlyApi");

            var payload = new { Email, Password };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiConfig.Auth.Login, content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Login failed.");
                return Page();
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseBody);
            var token = json.RootElement.GetProperty("token").GetString();

            // Save token to session (or cookie)
            HttpContext.Session.SetString("JWToken", token);

            return RedirectToPage("/Landlord/Index"); // Or wherever you'd like to go
        }
    }
}
