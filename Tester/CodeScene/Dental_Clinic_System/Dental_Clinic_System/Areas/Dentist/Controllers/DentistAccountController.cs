using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dental_Clinic_System.Areas.Dentist.Controllers
{
    [Area("Dentist")]
    public class DentistAccountController : Controller
    {
        private readonly DentalClinicDbContext _context;
        public DentistAccountController(DentalClinicDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/Dentist/DentistDetail/Index")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = "/Dentist/DentistDetail/Index")
        {
            var user = _context.Accounts.FirstOrDefault(d => username == d.Username && DataEncryptionExtensions.ToMd5Hash(password) == d.Password);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return BadRequest("Sai Ten Dang Nhap Hoac Mat Khau");
            }

            if (user.Role == "Nha Sĩ")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "DentistScheme");
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync("DentistScheme", new ClaimsPrincipal(claimsIdentity), authProperties);
                HttpContext.Session.SetInt32("dentistAccountID", user.ID);
                return RedirectToAction("index","dentistdetail", new { area = "dentist"});
            }

            ViewBag.ErrorMessage = "Invalid role";
            return NotFound("Account của bạn có Role không hợp lệ, vui lòng thử lại!");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("DentistScheme");
            return LocalRedirect("/dentist");
        }
    }
}
