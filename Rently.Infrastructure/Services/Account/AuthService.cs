using Microsoft.AspNetCore.Identity;
using Rently.Core.Constants;
using Rently.Core.Interfaces.Account;
using Rently.Core.Interfaces.Domain;
using Rently.Shared.Dtos.Auth;
using System.Net;

namespace Rently.Infrastructure.Services.Account
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IUserService userService,
            IAccountService landlordService)
        {
            _userManager = userManager;
            _userService = userService;
            _accountService = landlordService;
        }

        public async Task<ResultDto<string>> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing is not null)
                return ResultDto<string>.Failure(AccountMessages.UserAlreadyExists, HttpStatusCode.Conflict);

            var user = await _userService.CreateUserAsync(dto.Email, dto.Password, dto.PhoneNumber);
            var account = await _accountService.CreateAsync(dto, user.Id);
            await _userService.SendConfirmationEmailAsync(user);

            return ResultDto<string>.Success(AccountMessages.RegistrationSuccess);
        }

        public async Task<ResultDto<bool>> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ResultDto<bool>.Failure(AccountMessages.UserNotFound, HttpStatusCode.NotFound);

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            return result.Succeeded
                ? ResultDto<bool>.Success(true)
                : ResultDto<bool>.Failure(AccountMessages.EmailConfirmationFailed, HttpStatusCode.BadRequest);
        }

        public async Task<ResultDto<bool>> ChangePassword(ChangePasswordDto dto, Guid accountId)
        {
            var account = await _accountService.GetAccountAsync(accountId);

            var user = await _userManager.FindByIdAsync(account.IdentityUserId);

            if (user == null)
                return ResultDto<bool>.Failure(AccountMessages.UserNotFound, HttpStatusCode.NotFound);

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            return result.Succeeded
                ? ResultDto<bool>.Success(true)
                : ResultDto<bool>.Failure(AccountMessages.PasswordChangeFailed, HttpStatusCode.BadRequest);
        }

        public async Task<ResultDto<string>> ResendConfirmationEmailAsync(ResendEmailDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            const string genericResponse = AccountMessages.EmailConfirmationSent;

            if (user == null)
                return ResultDto<string>.Success(genericResponse); // Don’t reveal

            if (await _userManager.IsEmailConfirmedAsync(user))
                return ResultDto<string>.Success(AccountMessages.EmailAlreadyConfirmed);

            await _userService.SendConfirmationEmailAsync(user);
            return ResultDto<string>.Success(genericResponse);
        }

        public async Task<ResultDto<AuthDto>> AuthenticateUser(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return ResultDto<AuthDto>.Failure(AccountMessages.InvalidCredentials, HttpStatusCode.Unauthorized);

            if (!user.EmailConfirmed)
                return ResultDto<AuthDto>.Failure(AccountMessages.EmailNotConfirmed, HttpStatusCode.Forbidden);

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return ResultDto<AuthDto>.Failure(AccountMessages.InvalidCredentials, HttpStatusCode.Unauthorized);

            var account = await _accountService.GetAccountByIdentityAsync(user.Id);

            var authDto = new AuthDto
            {
                Email = dto.Email,
                FullName = $"{account.FirstName} {account.LastName}",
                AccountId = account.Id.ToString(),
            };

            return ResultDto<AuthDto>.Success(authDto);
        }
    }
}
