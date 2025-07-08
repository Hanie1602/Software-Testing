using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Dental_Clinic_System.Controllers
{
    public class ClinicController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public ClinicController(DentalClinicDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clinics = await _context.Clinics.ToListAsync();
            return View("clinic", clinics);
        }

        [HttpGet]
        public async Task<IActionResult> ClinicDetail(int clinicID)
        {
            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ID == clinicID);
            if (clinic == null)
            {
                return NotFound();
            }

            // Get Review from Patient
            var reviews = await _context.Reviews.Include(r => r.Patient).Include(r => r.Dentist).Where(d => d.Dentist.ClinicID == clinicID).ToListAsync();
            ViewBag.Reviews = reviews;

            return View(clinic);
        }

        [HttpGet]
        public async Task<IActionResult> ChooseClinicSpecialty(int clinicID)
        {
            var clinic = await _context.Clinics
                .Include(c => c.Dentists)
                    .ThenInclude(d => d.Account)
                .Include(c => c.Dentists)
                    .ThenInclude(d => d.DentistSpecialties)
                    .ThenInclude(ds => ds.Specialty)
                .FirstOrDefaultAsync(c => c.ID == clinicID);

            if (clinic == null)
            {
                return NotFound();
            }

            var dentistAvailable = clinic.Dentists
                .Where(d => d.Account.AccountStatus == "Hoạt Động")
                .ToList();

            var specialties = dentistAvailable
                .SelectMany(d => d.DentistSpecialties)
                .Select(ds => ds.Specialty)
                .Distinct()
                .ToList();

            ViewBag.ClinicID = clinicID;

            return View("ClinicSpecialty", specialties);
        }

        [HttpGet]
        public async Task<IActionResult> ChooseDentist(int clinicID, int specialtyID, string keysearch = null, int? degreeID = null, string gender = null)
        {
            var dentistsQuery = _context.Dentists
                .Include(d => d.Account)
                .Include(d => d.Clinic)
                .Include(d => d.DentistSpecialties)
                    .ThenInclude(ds => ds.Specialty)
                .Include(d => d.Degree)
                .Where(d => d.ClinicID == clinicID && d.Account.AccountStatus == "Hoạt Động" && d.DentistSpecialties.Any(ds => ds.SpecialtyID == specialtyID));

            if (degreeID != null)
            {
                dentistsQuery = dentistsQuery.Where(d => d.Degree.ID == degreeID);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                dentistsQuery = dentistsQuery.Where(d => d.Account.Gender == gender);
            }

            if (!string.IsNullOrEmpty(keysearch))
            {
                dentistsQuery = dentistsQuery.Where(d => d.Account.LastName.ToLower().Contains(keysearch.ToLower()) || d.Account.FirstName.ToLower().Contains(keysearch.ToLower()));
            }

            var dentists = await dentistsQuery.ToListAsync();

            var firstDentistSpecialty = dentists.FirstOrDefault()?.DentistSpecialties.FirstOrDefault(ds => ds.SpecialtyID == specialtyID)?.Specialty;

            ViewBag.Specialty = firstDentistSpecialty;
            ViewBag.clinicID = clinicID;
            ViewBag.specialtyID = specialtyID;
            ViewBag.degree = degreeID;
            ViewBag.gender = gender;
            ViewBag.keysearch = keysearch;

            return View("ClinicDentist", dentists);
        }

        [HttpGet]
        public async Task<IActionResult> ClinicDentistCalendar(int clinicID, int specialtyID, int dentistID)
        {
            var schedule = await _context.Schedules
                .Include(s => s.TimeSlot)
                .Include(s => s.Appointments)
                .Include(s => s.Dentist)
                    .ThenInclude(d => d.DentistSpecialties)
                    .ThenInclude(ds => ds.Specialty)
                .Where(s => s.DentistID == dentistID)
                .ToListAsync();

            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ID == clinicID);
            var specialty = await _context.Specialties.FirstOrDefaultAsync(s => s.ID == specialtyID);
            var dentist = await _context.Dentists.Include(d => d.Account).FirstOrDefaultAsync(d => d.ID == dentistID);

            if (clinic == null || specialty == null || dentist == null)
            {
                return NotFound();
            }

            ViewBag.clinicName = clinic.Name;
            ViewBag.clinicAddress = $"{clinic.Address}, {clinic.WardName}, {clinic.DistrictName}, {clinic.ProvinceName}";
            ViewBag.dentistName = $"{dentist.Account.LastName} {dentist.Account.FirstName}";
            ViewBag.specialtyName = specialty.Name;
            ViewBag.clinicID = clinicID;
            ViewBag.specialtyID = specialtyID;
            ViewBag.dentistID = dentistID;

            return View(schedule);
        }

        [HttpGet]
        public IActionResult GetSchedules(int dentistID)
        {
            var schedules = _context.Schedules
                .Include(s => s.TimeSlot)
                .Include(s => s.Dentist)
                .Where(s => s.Dentist.ID == dentistID && s.ScheduleStatus == "Còn Trống")
                .Select(s => new
                {
                    Date = s.Date.ToString("yyyy-MM-dd"),
                    StartTime = s.TimeSlot.StartTime.ToString(@"hh\:mm"),
                    EndTime = s.TimeSlot.EndTime.ToString(@"hh\:mm"),
                    scheduleID = s.ID
                })
                .ToList();

            foreach (var s in schedules)
            {
                Console.WriteLine(s);
            }

            return Json(schedules);
        }
    }
}
