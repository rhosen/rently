using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;

namespace Rently.App.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public ConfirmEmailModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            {
                StatusMessage = "Invalid confirmation link.";
                return Page();
            }

            var client = _clientFactory.CreateClient("RentlyApi");
            var url = $"{ApiConfig.Auth.ConfirmEmail}{UserId}&token={Uri.EscapeDataString(Token)}";

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Email confirmed successfully!";
            }
            else
            {
                StatusMessage = "Email confirmation failed.";
            }

            return Page();
        }
    }

}

