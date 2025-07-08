using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dental_Clinic_System.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminAccountController : Controller
	{
		private readonly DentalClinicDbContext _context;

		public AdminAccountController(DentalClinicDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Login(string returnUrl = "/Admin/Dashboard/Index")
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(string username, string password)
		{
			var user = _context.Accounts.FirstOrDefault(d => username == d.Username && password == d.Password);
			if (user == null)
			{
				ViewBag.ErrorMessage = "Invalid username or password";
				return BadRequest("Sai Tên đăng nhập hoặc Mật khẩu");
			}

			if (user.Role == "Admin")
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.Username),
					new Claim(ClaimTypes.Role, user.Role)
				};

				var claimsIdentity = new ClaimsIdentity(claims, "GetAppointmentStatus");
				var authProperties = new AuthenticationProperties { IsPersistent = true };

				await HttpContext.SignInAsync("GetAppointmentStatus", new ClaimsPrincipal(claimsIdentity), authProperties);
				HttpContext.Session.SetInt32("adminAccountID", user.ID);
				return RedirectToAction("index", "dashboard", new { area = "admin" });
			}

			ViewBag.ErrorMessage = "Invalid role";
			return NotFound("Account của bạn có Role không hợp lệ, vui lòng thử lại!");
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync("GetAppointmentStatus");
			return LocalRedirect("/admin");
		}
	}
}
