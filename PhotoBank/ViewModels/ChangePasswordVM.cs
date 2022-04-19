using System.ComponentModel.DataAnnotations;

namespace PhotoBank.ViewModels
{
    public class ChangePasswordVM
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Enter your password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Enter your new password")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Confirm your password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
