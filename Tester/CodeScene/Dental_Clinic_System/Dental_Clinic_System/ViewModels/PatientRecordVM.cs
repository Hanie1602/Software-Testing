using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.ViewModels
{
	public class PatientRecordVM
	{
		[Display(Name = "Họ và Tên")]
		[Required(ErrorMessage = "Vui lòng nhập đầy đủ họ tên!")]
		[RegularExpression(@"^[\p{L} ]*$", ErrorMessage = "Tên chỉ được chứa chữ cái và dấu cách!")]
		public string FullName { get; set; }


		[Display(Name = "Ngày sinh")]
		[Required(ErrorMessage = "Vui lòng chọn ngày tháng năm sinh!")]
		public DateOnly DateOfBirdth { get; set; }

		[Display(Name = "Số điện thoại")]
		[Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ!")]
		[RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại phải có độ dài từ 9 đến 11 ký tự và chỉ chứa các chữ số.")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Giới tính")]
		[Required(ErrorMessage = "Vui lòng chọn giới tính!")]
		public string Gender { get; set; }

		[Display(Name = "Nghề nghiệp")]
		[Required(ErrorMessage = "Vui lòng chọn nghề nghiệp!")]
		public string Job { get; set; }

		[Display(Name = "Mã định danh")]
		[RegularExpression(@"^\d{12}$", ErrorMessage = "Số CCCD phải có độ dài 12 ký tự và chỉ chứa các chữ số.")]
		public string? IdentifyNumber { get; set; }

		[Display(Name = "Email")]
		[EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
		[StringLength(50, ErrorMessage = "Email không được vượt quá 50 ký tự")]
		public string? Email { get; set; } = null;

		[Display(Name = "Tỉnh thành")]
		[Required(ErrorMessage = "Vui lòng chọn tỉnh / thành phố của bạn!")]
		public int Province {  get; set; }

		[Display(Name = "Quận huyện")]
		[Required(ErrorMessage = "Vui lòng chọn quận / huyện của bạn!")]
		public int District { get; set; }

		[Display(Name = "Phường xã")]
		[Required(ErrorMessage = "Vui lòng chọn phường / xã của bạn!")]
		public int Ward { get; set; }

		[Display(Name = "Địa chỉ")]
		[Required(ErrorMessage = "Vui lòng nhập địa chỉ cụ thể của bạn!")]
		public string Address { get; set; }

		//Định nghĩa các Relationship liên quan đến patient record và cho nó giá trị mặc định là null
		[Display(Name = "Họ tên nhân thân")]
		[RegularExpression(@"^[\p{L} ]*$", ErrorMessage = "Tên chỉ được chứa chữ cái và dấu cách!")]
		public string? FMFullName { get; set; } = null;

		[Display(Name = "Quan hệ với bệnh nhân")]
		public string? FMRelationship { get; set; } = null;

		[Display(Name = "Số điện thoại")]
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ!")]
		[RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại phải có độ dài từ 9 đến 11 ký tự và chỉ chứa các chữ số.")]
		public string? FMPhoneNumber { get; set; } = null;

		[Display(Name = "Email")]
		[EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
		[StringLength(50, ErrorMessage = "Email không được vượt quá 50 ký tự")]
		public string? FMEmail { get; set; } = null;
	}
}
