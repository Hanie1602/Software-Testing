using Dental_Clinic_System.Areas.Admin.ViewModels;
using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dental_Clinic_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "GetAppointmentStatus", Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public DashboardController(DentalClinicDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TempData.Keep("userID");
            return RedirectToAction("GetAppointmentStatus");
        }


        public async Task<IActionResult> GetAppointmentStatus(int? year)
        {
            var adminAccountID = HttpContext.Session.GetInt32("adminAccountID");
            if (adminAccountID == null)
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            int currentYear = year ?? DateTime.Now.Year;
            DateTime today = DateTime.Today;

            //Đặt khám thành công/thất bại mỗi tháng
            var successfulAppointmentsPerMonth = await _context.Appointments
                .Where(a => a.CreatedDate.HasValue && a.CreatedDate.Value.Year == currentYear && a.AppointmentStatus == "Đã Khám")
                .GroupBy(a => a.CreatedDate.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var failedAppointmentsPerMonth = await _context.Appointments
                .Where(a => a.CreatedDate.HasValue && a.CreatedDate.Value.Year == currentYear && a.AppointmentStatus == "Đã Hủy")
                .GroupBy(a => a.CreatedDate.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var successfulData = new int[12];
            var failedData = new int[12];

            foreach (var item in successfulAppointmentsPerMonth)
            {
                successfulData[item.Month - 1] = item.Count;
            }

            foreach (var item in failedAppointmentsPerMonth)
            {
                failedData[item.Month - 1] = item.Count;
            }

            //Đặt khám thành công/thất bại trong ngày
            var successfulAppointmentsToday = await _context.Appointments
                .Where(a => a.CreatedDate.HasValue && a.CreatedDate.Value.Date == today && a.AppointmentStatus == "Đã Khám")
                .CountAsync();

            var failedAppointmentsToday = await _context.Appointments
                .Where(a => a.CreatedDate.HasValue && a.CreatedDate.Value.Date == today && a.AppointmentStatus == "Đã Hủy")
                .CountAsync();

            var model = new AppointmentVM
            {
                SelectedYear = currentYear,
                SuccessfulAppointments = successfulAppointmentsPerMonth.Sum(x => x.Count),
                FailedAppointments = failedAppointmentsPerMonth.Sum(x => x.Count),
                MonthlySuccessfulAppointments = successfulData.ToList(),
                MonthlyFailedAppointments = failedData.ToList(),
                SuccessfulAppointmentsToday = successfulAppointmentsToday,
                FailedAppointmentsToday = failedAppointmentsToday
            };

            Console.WriteLine("SuccessfulAppointmentsToday: " + successfulAppointmentsToday);
            Console.WriteLine("FailedAppointmentsToday: " + failedAppointmentsToday);

            return View(model);
        }

    }

}
