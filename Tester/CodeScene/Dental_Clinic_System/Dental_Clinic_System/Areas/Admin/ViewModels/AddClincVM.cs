using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Admin.ViewModels
{
    public class AddClincVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng tên phòng khám.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng tải hình ảnh.")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^\d{8,11}$", ErrorMessage = "Số điện thoại phải có độ dài từ 8 đến 11 số.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(50, ErrorMessage = "Email không được quá 50 ký tự.")]
        [MinLength(3, ErrorMessage = "Email phải có ít nhất 3 ký tự.")]
        public string Email { get; set; }

        public string Basis { get; set; }

        public string? ClinicStatus { get; set; }

        //public string MapLinker { get; set; }

        public int ManagerID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Tỉnh/ Thành phố.")]
        public int? Province { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Quận/ Huyện.")]
        public int? District { get; set; }

        public int? Ward { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        public string Address { get; set; }

        public int AmWorkTimeID { get; set; }

        public int PmWorkTimeID { get; set; }

        public SelectList? AmWorkTimes { get; set; }

        public SelectList? PmWorkTimes { get; set; }

        public SelectList? UnassignedManagers { get; set; }


    }
}
