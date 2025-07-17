using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Rently.Core.Interfaces.Account;
using Rently.Core.Interfaces.Domain;
using Rently.Shared.Dtos.Auth;
using System.Net;

namespace Rently.Infrastructure.Services.Account
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserService _userService;
        private readonly ILandlordService _landlordService;

        public AccountService(
            UserManager<IdentityUser> userManager,
            IUserService userService,
            ILandlordService landlordService)
        {
            _userManager = userManager;
            _userService = userService;
            _landlordService = landlordService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing is not null)
                throw new InvalidOperationException("User already exists");

            var user = await _userService.CreateUserAsync(dto.Email, dto.Password, dto.PhoneNumber);

            var landlord = await _landlordService.CreateAsync(dto, user.Id);

            await _userService.SendConfirmationEmailAsync(user);

            return "Registration successful. Please confirm your email.";
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(ChangePasswordDto dto, string identityId)
        {
            var user = await _userManager.FindByIdAsync(identityId);

            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            return result.Succeeded;

        }

        public async Task<string> ResendConfirmationEmailAsync(ResendEmailDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return "If your email is registered, a confirmation link has been sent."; // Don't reveal user existence

            if (await _userManager.IsEmailConfirmedAsync(user))
                return "Email is already confirmed. You can login.";

            await _userService.SendConfirmationEmailAsync(user);

            return "If your email is registered, a confirmation link has been sent.";
        }

        public async Task<ResultDto<AuthDto>> AuthenticateUser(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return ResultDto<AuthDto>.Failure("Invalid email or password");

            if (!user.EmailConfirmed)
                return ResultDto<AuthDto>.Failure("Email not confirmed");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return ResultDto<AuthDto>.Failure("Invalid email or password");

            var landlord = await _landlordService.GetByIdentityUserIdAsync(user.Id);

            var authDto = new AuthDto
            {
                Email = dto.Email,
                FullName = string.Concat(landlord.FirstName, " ", landlord.LastName),
                LandlordId = landlord.Id.ToString(),
            };

            return ResultDto<AuthDto>.Success(authDto);
        }
    }
}

