using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [Display(Name = "Địa chỉ Email đã đăng ký")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
