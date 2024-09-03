using System.ComponentModel.DataAnnotations;

namespace CozaStore.ViewModels
{
    public class ChangePasswordVM
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public required string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }
    }
}
