using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Admin.ViewModels
{
	public class ManagerSpecialtyVM
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên chuyên khoa.")]
		[StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        public string Name { get; set; } = null!;

		public string Image { get; set; } 

		public string? Description { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tiền cọc.")]
		[Range(0, double.MaxValue, ErrorMessage = "Tiền cọc phải lớn hơn 0!!")]
		public decimal Deposit {  get; set; }

    }
}
