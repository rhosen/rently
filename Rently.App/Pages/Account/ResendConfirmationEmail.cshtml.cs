using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using System.Text.Json;

namespace Rently.App.Pages.Account
{
    public class ResendConfirmationEmailModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public ResendConfirmationEmailModel(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Email { get; set; }
        public string StatusMessage { get; set; }

        public void OnGet()
        {
            // Just display the form
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                StatusMessage = "Please enter your email address.";
                return Page();
            }

            var client = _clientFactory.CreateClient("RentlyApi");
            var content = new StringContent(JsonSerializer.Serialize(new { Email }), System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(ApiConfig.Auth.ReconfirmEmail, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Account/RegistrationConfirmation");
                }
                else
                {
                    StatusMessage = "Unable to send confirmation email. Please try again later.";
                }
            }
            catch
            {
                StatusMessage = "An error occurred. Please try again later.";
            }

            return Page();
        }
    }
}
