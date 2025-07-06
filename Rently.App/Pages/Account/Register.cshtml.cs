using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using System.Text;
using System.Text.Json;

namespace Rently.App.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("RentlyApi");

            var payload = new
            {
                Email,
                Password,
                Name = Email // Adjust if you support real names
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiConfig.Auth.Register, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Account/Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed.");
            return Page();
        }
    }
}
