using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.ViewModels;
using Dental_Clinic_System.Areas.Manager.ViewModels;

namespace Dental_Clinic_System.Controllers
{
    [Area("Manager")]
   // [Route("Manager/[controller]")]
    public class ServiceController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public ServiceController(DentalClinicDbContext context)
        {
            _context = context;
        }

       // [Route("Index")]
        // GET: Service
        public async Task<IActionResult> Index()
        {
            var serviceList = _context.Services.Include(s => s.Clinic).Include(s => s.Specialty);
            return View(await serviceList.ToListAsync());
        }
       // [Route("Search")]
        public async Task<IActionResult> Search(String keyword)
		{
            var serviceList = _context.Services.Include(s => s.Clinic).Include(s => s.Specialty).AsQueryable();
            if (keyword != null)
            {
                keyword = keyword.Trim();
                serviceList = serviceList.Where(p =>
                    (p.Specialty.Name.Contains(keyword)) ||
                    p.Name.Contains(keyword));
            }
            return View("Index", await serviceList.ToListAsync());
		}

		// GET: Service/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Clinic)
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

		//[Route("Create")]
		// GET: Service/Create
		public IActionResult Create()
        {
            ViewData["ClinicID"] = new SelectList(_context.Clinics, "ID", "Name");
            ViewData["SpecialtyID"] = new SelectList(_context.Specialties, "ID", "Name");
            return View();
        }

        // POST: Service/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClinicID,SpecialtyID,Name,Description,Price")] ServiceVM service)
        {
            if (ModelState.IsValid)
            {
                var newSer = new Service
                {
                    ClinicID = service.ClinicID,
                    SpecialtyID = service.SpecialtyID,
                    Name = service.Name,
                    Description = service.Description,
                    Price = service.Price
                };
                _context.Add(newSer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Debugging
            //Console.WriteLine($"ClinicID: {service.ClinicID}");
            //Console.WriteLine($"SpecialtyID: {service.SpecialtyID}");
            ViewData["ClinicID"] = new SelectList(_context.Clinics, "ID", "Name", service.ClinicID);
            ViewData["SpecialtyID"] = new SelectList(_context.Specialties, "ID", "Name", service.SpecialtyID);

            //List<string> errors = new List<string>();
            //foreach(var value in ModelState.Values)
            //{
            //    foreach(var error in value.Errors)
            //    {
            //        errors.Add(error.ErrorMessage);
            //    }
            //}
            //string errorMessage = string.Join("\n", errors);
            //return BadRequest(errorMessage);
            return View(service);
        }

        // GET: Service/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["ClinicID"] = new SelectList(_context.Clinics, "ID", "Name", service.ClinicID);
            ViewData["SpecialtyID"] = new SelectList(_context.Specialties, "ID", "Name", service.SpecialtyID);
            return View(service);
        }

        // POST: Service/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClinicID,SpecialtyID,Name,Description,Price")] ServiceVM service)
        {
            if (id != service.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var upSer = await _context.Services.FindAsync(id);
                    if (upSer != null)
                    {
                        upSer.SpecialtyID = service.SpecialtyID;
                        upSer.Name = service.Name;
                        upSer.Description = service.Description;
                        upSer.Price = service.Price;
                    }
                    _context.Update(upSer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = id });
            }
            ViewData["ClinicID"] = new SelectList(_context.Clinics, "ID", "Name", service.ClinicID);
            ViewData["SpecialtyID"] = new SelectList(_context.Specialties, "ID", "Name", service.SpecialtyID);
            return View(service);
        }

        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Clinic)
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ID == id);
        }
    }
}
