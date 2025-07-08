using Dental_Clinic_System.Models;
using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Dental_Clinic_System.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly DentalClinicDbContext _context;

		public HomeController(ILogger<HomeController> logger, DentalClinicDbContext context)
		{
			_logger = logger;
			_context = context;	
		}

		public IActionResult Index()
		{

            var specialties = _context.Specialties.ToList();
            ViewBag.Specialities = specialties;

            var clinics = _context.Clinics.ToList();
            ViewBag.Clinics = clinics;

            var claimsValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

			if (claimsValue == null)
			{
				return View();
			}

			else if (_context.Accounts.FirstOrDefault(u => u.Email == claimsValue) == null)
			{
				HttpContext.SignOutAsync();
				return View();
			}
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
