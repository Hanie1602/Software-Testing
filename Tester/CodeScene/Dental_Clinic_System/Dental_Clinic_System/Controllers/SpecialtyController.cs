using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dental_Clinic_System.Controllers
{
	public class SpecialtyController : Controller
	{
		DentalClinicDbContext _context;
        public SpecialtyController(DentalClinicDbContext context)
        {
			_context = context;
        }

        public IActionResult Index()
		{
			var specialties = _context.Specialties.ToList();
			return View("Specialty",specialties);
		}

		[HttpGet]
		public IActionResult ChooseDentistry(int specialtyID)
		{
			var dentistry = _context.Dentists
									.Include(d => d.DentistSpecialties).ThenInclude( ds => ds.Specialty)
									.Include(d => d.Account)
									.Include(d => d.Clinic)
									.Include(d => d.Schedules).ThenInclude(s => s.TimeSlot)
									.Where(w => w.DentistSpecialties.Any(c => c.SpecialtyID == specialtyID))
									.ToList();

			ViewBag.SpecialtyID = specialtyID;

			return View("Dentistry",dentistry);
		}

		[HttpGet]
		public IActionResult ShowDentist(int specialtyID, int dentistID)
		{
			var dentistInfo = _context.Dentists
									.Include(d => d.DentistSpecialties).ThenInclude(ds => ds.Specialty)
									.Include(d => d.Account)
									.Include(d => d.Clinic)
									.Where(w => w.ID == dentistID && w.DentistSpecialties.Any(c => c.SpecialtyID == specialtyID))
									.FirstOrDefault();
			ViewBag.specialtyID = specialtyID;
			ViewBag.dentistID = dentistID;
			return View("DentistInformation", dentistInfo);
		}

		[HttpGet]
		//[Authorize(Roles ="Nha Sĩ")]
		public async Task<IActionResult> BookDentist(int specialtyID, int dentistID)
		{
			if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

			var userID = User.Identity.Name;

			var dentistInfo = _context.Dentists
									  .Include(d => d.DentistSpecialties)
										.ThenInclude(ds => ds.Specialty)
									  .Include(d => d.Account)
									  .Include(d => d.Clinic)
									  .Where(d => d.ID == dentistID && d.DentistSpecialties.FirstOrDefault().Specialty.ID == specialtyID)
									  .FirstOrDefault();
			ViewBag.userName = userID;
			ViewBag.specialtyID = specialtyID;
			ViewBag.dentistID = dentistID;
			return View("DentistBookingRequest");
		}

		public IActionResult Test(APILocalVM city)
		{
			var test = new APILocalVM { Province = city.Province , District = city.District , Ward = city.Ward};
			Console.WriteLine($"{test.Province} | {test.District} | {test.Ward}");
			return RedirectToAction("Index", "Home");
		}
	}
}
