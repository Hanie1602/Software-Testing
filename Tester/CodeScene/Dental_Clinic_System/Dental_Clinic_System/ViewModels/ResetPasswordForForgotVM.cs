using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.ViewModels
{
    public class ResetPasswordForForgotVM
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
