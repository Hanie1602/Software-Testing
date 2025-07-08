using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dental_Clinic_System.Controllers
{
    public class NewsController : Controller
    {
        private readonly DentalClinicDbContext _context;
        public NewsController(DentalClinicDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var newsList = await _context.News.ToListAsync();
            return View(newsList);
        }

        [HttpGet]
        public async Task<IActionResult> NewsDetail(int newsID)
        {
            await Console.Out.WriteLineAsync("99999999999999999999999999");
            await Console.Out.WriteLineAsync($"NewsID = {newsID}");
            var news = await _context.News.FirstOrDefaultAsync(c => c.ID == newsID);
            string? content = news?.Content;

            if(news == null)
            {
                await Console.Out.WriteLineAsync("----------------------------");
                await Console.Out.WriteLineAsync("nulllllllllllllllllllll from newsdetail controller");
                await Console.Out.WriteLineAsync("----------------------------");
            } else
            {
                await Console.Out.WriteLineAsync("----------------------------");
                await Console.Out.WriteLineAsync("NOTTTTTTTTTTTT nulllllllllllllllllllll from newsdetail controller");
                await Console.Out.WriteLineAsync("----------------------------");
            }

            ViewBag.News = news;

            return View();
        }
    }
}
