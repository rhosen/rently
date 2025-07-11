using Microsoft.AspNetCore.Mvc;
using Rently.Core.Interfaces.Account;
using Rently.Core.Interfaces.Utils;
using Rently.Infrastructure.Services.Account;
using Rently.Shared.Dtos.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rently.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IAccountService accountService,
                              ITokenGenerator tokenGenerator)
        {
            _accountService = accountService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _accountService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _accountService.AuthenticateUser(dto);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, result.LandlordId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, result.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Name, result.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = _tokenGenerator.GenerateToken(claims);

            return Ok(new { token });
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendEmailDto dto)
        {
            var result = await _accountService.ResendConfirmationEmailAsync(dto); 
            return Ok(result);
        }
    }
}
