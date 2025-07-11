using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rently.App.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            return User.Identity.IsAuthenticated ? RedirectToPage("/Landlord/Index") : Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await AuthenticateUserAsync(Email, Password);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden &&
                    responseContent.Contains("Email not confirmed"))
                {
                    return RedirectToPage("/Account/ResendConfirmationEmail");
                }

                ModelState.AddModelError(string.Empty, "Login failed.");
                return Page();
            }


            var token = await ExtractTokenAsync(response);

            var claims = CreateCookieClaims(token);

            await SignInUserAsync(claims);

            return RedirectToPage("/Landlord/Index");
        }

        private async Task<HttpResponseMessage> AuthenticateUserAsync(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("RentlyApi");
            var payload = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            return await client.PostAsync(ApiConfig.Auth.Login, content);
        }

        private async Task<string> ExtractTokenAsync(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseBody);
            return json.RootElement.GetProperty("token").GetString();
        }

        private List<Claim> CreateCookieClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userId = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var email = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            var fullName = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId ?? ""),
                new Claim(ClaimTypes.Email, email ?? ""),
                new Claim(ClaimTypes.Name, fullName ?? ""),
                new Claim("JWToken", token ?? "")
            };
        }


        private async Task SignInUserAsync(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}
