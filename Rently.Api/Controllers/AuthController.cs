using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rently.Api.Data;
using Rently.Api.Data.Entities;
using Rently.Common.Dtos.Auth;
using Rently.Common.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppEmailSender = Rently.Core.Interfaces.Messaging.IEmailSender;

namespace Rently.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RentlyDbContext _context;
        private readonly FrontendOptions _frontendOptions;
        private readonly AppEmailSender _emailSender;

        public AuthController(UserManager<IdentityUser> userManager,
                              IConfiguration configuration,
                              RentlyDbContext context,
                              IOptions<FrontendOptions> frontendOptions,
                              AppEmailSender emailSender)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _frontendOptions = frontendOptions.Value;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Check if user already exists
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return BadRequest("User already exists!");

            // Create Identity user
            var user = new IdentityUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Create Landlord record
            var landlord = new Landlord
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,

                StreetAddress = dto.StreetAddress,
                City = dto.City,
                StateOrProvince = dto.StateOrProvince,
                PostalCode = dto.PostalCode,
                Country = dto.Country,

                IdentityUserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Landlords.Add(landlord);
            await _context.SaveChangesAsync();

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"{_frontendOptions.BaseUrl}/Account/ConfirmEmail?userId={user.Id}&token={token}";

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

            return Ok("User created successfully! Please check your email to confirm your account.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            // Check email confirmed before allowing login
            if (!user.EmailConfirmed)
                return StatusCode(403, "Email not confirmed");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return Unauthorized("Invalid email or password");

            var landlord = await _context.Landlords.SingleOrDefaultAsync(x => x.IdentityUserId == user.Id);

            var token = GenerateJwtToken(user, landlord);

            return Ok(new { token });
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendEmailDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
                return BadRequest("Email is required.");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Ok("If your email is registered, a confirmation link has been sent."); // Don't reveal user existence

            if (await _userManager.IsEmailConfirmedAsync(user))
                return Ok("Email is already confirmed. You can login.");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"{_frontendOptions.BaseUrl}/Account/ConfirmEmail?userId={user.Id}&token={token}";

            var emailBody = $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";

            await _emailSender.SendEmailAsync(dto.Email, "Confirm your email", emailBody);

            return Ok("If your email is registered, a confirmation link has been sent.");
        }

        private string GenerateJwtToken(IdentityUser user, Landlord landord)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, landord.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Name, string.Concat(landord.FirstName, " ", landord.LastName) ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }

}
