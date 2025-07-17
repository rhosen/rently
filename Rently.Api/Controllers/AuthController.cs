using Microsoft.AspNetCore.Mvc;
using Rently.Core.Interfaces.Account;
using Rently.Core.Interfaces.Utils;
using Rently.Shared.Dtos.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rently.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _accountService;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IAuthService accountService,
                              ITokenGenerator tokenGenerator)
        {
            _accountService = accountService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _accountService.RegisterAsync(dto);
            if (result.IsSuccess) return Ok(result.Data);
            return StatusCode((int)result.StatusCode, result.ErrorMessage);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _accountService.ConfirmEmailAsync(userId, token);
            if (result.IsSuccess) return Ok(result.Data);
            return StatusCode((int)result.StatusCode, result.ErrorMessage);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _accountService.AuthenticateUser(dto);

            if (!result.IsSuccess) return StatusCode((int)result.StatusCode, result.ErrorMessage);

            var account = result.Data;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Name, account.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = _tokenGenerator.GenerateToken(claims);

            return Ok(new { token });
        }


        [HttpPost("resend-email-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendEmailDto dto)
        {
            var result = await _accountService.ResendConfirmationEmailAsync(dto);
            if (result.IsSuccess) return Ok(result.Data);
            return StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
    }
}
