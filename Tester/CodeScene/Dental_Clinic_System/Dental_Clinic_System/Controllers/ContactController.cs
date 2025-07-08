using Microsoft.AspNetCore.Mvc;

namespace Dental_Clinic_System.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View("Contact");
		}
	}
}
