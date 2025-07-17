using Microsoft.AspNetCore.Identity;
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

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid user ID or token.");

            var result = await _accountService.ConfirmEmailAsync(userId, token);

            if (result)
                return Ok("Email confirmed.");
            else
                return BadRequest("Email confirmation failed.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var authResult = await _accountService.AuthenticateUser(dto);

            if (!authResult.IsSuccess)
            {
                if (authResult.ErrorMessage == "Email not confirmed")
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = authResult.ErrorMessage });

                return Unauthorized(new { message = authResult.ErrorMessage });
            }

            var result = authResult.Data!;

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
