using Microsoft.AspNetCore.Mvc;

namespace Dental_Clinic_System.Controllers
{
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
            return View("About");
		}
	}
}
