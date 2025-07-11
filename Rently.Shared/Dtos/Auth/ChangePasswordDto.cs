using System.ComponentModel.DataAnnotations;

namespace Rently.Shared.Dtos.Auth
{
    public class ChangePasswordDto
    {

        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
