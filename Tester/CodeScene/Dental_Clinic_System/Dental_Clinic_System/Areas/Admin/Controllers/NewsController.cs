using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace Dental_Clinic_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly DentalClinicDbContext _context;
        public NewsController(DentalClinicDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> NewsPost()
        {
            var user = _context.Accounts.FirstOrDefault(r => r.Role == "Admin");

            ViewBag.AdminID = user?.ID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewsPost(string? content, string? newsTitle, int adminID)
        {
            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Define the UTC+7 time zone
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Convert the UTC time to UTC+7
            DateTime utcPlus7Now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);

            var news = new News
            {
                AccountID = adminID,
                Title = newsTitle,
                Content = content,
                CreatedDate = utcPlus7Now,
                ThumbNail = null,
                Status = null
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return RedirectToAction("newspost", "news", "admin");
        }
    }
}
