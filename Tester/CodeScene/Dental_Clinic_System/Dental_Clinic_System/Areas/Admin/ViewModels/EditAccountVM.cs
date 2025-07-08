using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Admin.ViewModels
{
	public class EditAccountVM
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
		[StringLength(30, ErrorMessage = "Tên đăng nhập không được vượt quá 30 ký tự.")]
		[MinLength(3, ErrorMessage = "Tên đăng nhập phải có ít nhất 3 ký tự.")]
		[RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Tên đăng nhập chỉ được chứa các ký tự chữ cái và số, và phải bắt đầu bằng chữ cái.")]
		public string Username { get; set; } = null!;

		//[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		//[DataType(DataType.Password)]
		//[StringLength(30, ErrorMessage = "Mật khẩu không được quá 30 ký tự.")]
		//[MinLength(3, ErrorMessage = "Mật khẩu phải có ít nhất 3 ký tự.")]
		//[RegularExpression("[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Mật khẩu chỉ được chứa các ký tự chữ cái và số, và phải bắt đầu bằng chữ cái.")]
		//public string Password { get; set; } = null!;

		[DataType(DataType.Password)]
		[StringLength(30, ErrorMessage = "Mật khẩu không được quá 30 ký tự.")]
		[MinLength(3, ErrorMessage = "Mật khẩu phải có ít nhất 3 ký tự.")]
		[RegularExpression("[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Mật khẩu chỉ được chứa các ký tự chữ cái và số, và phải bắt đầu bằng chữ cái.")]
		public string? NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
		public string? ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập email.")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ.")]
		[StringLength(50, ErrorMessage = "Email không được quá 50 ký tự.")]
		[MinLength(3, ErrorMessage = "Email phải có ít nhất 3 ký tự.")]
		public string? Email { get; set; }

		public string? Gender { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
		[RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại phải có độ dài từ 9 đến 11 số.")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
		public DateOnly? DateOfBirth { get; set; }

		public int? Province { get; set; }

		public int? District { get; set; }

		public int? Ward { get; set; }

		public string? Address { get; set; }

		public string Role { get; set; }

		public int DegreeID { get; set; }

		public int ClinicID { get; set; }
	}
}
