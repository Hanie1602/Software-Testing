using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.Areas.Manager.ViewModels;

namespace Dental_Clinic_System.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ClinicsController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public ClinicsController(DentalClinicDbContext context)
        {
            _context = context;
        }

        // GET: Manager/Clinics
        public async Task<IActionResult> Index()
        {
            var dentalClinicDbContext = _context.Clinics.Include(c => c.Manager);
            return View(await dentalClinicDbContext.ToListAsync());
        }

        // GET: Manager/Clinics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics
                .Include(c => c.Manager)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // GET: Manager/Clinics/Create
        public IActionResult Create()
        {
            ViewData["ManagerID"] = new SelectList(_context.Accounts, "ID", "AccountStatus");
            return View();
        }

        // POST: Manager/Clinics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ManagerID,Name,Province,Ward,District,Address,Basis,PhoneNumber,Email,Description,Image,ClinicStatus")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clinic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerID"] = new SelectList(_context.Accounts, "ID", "AccountStatus", clinic.ManagerID);
            return View(clinic);
        }

        // GET: Manager/Clinics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }
            ViewData["ManagerID"] = new SelectList(_context.Accounts, "ID", "AccountStatus", clinic.ManagerID);
            return View(clinic);
        }

        // POST: Manager/Clinics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ManagerID,Name,Province,Ward,District,Address,Basis,PhoneNumber,Email,Description,Image,ClinicStatus")] ClinicVM clinic)
        {
            if (id != clinic.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var upClinic = await _context.Clinics.FindAsync(id);
                    if (upClinic != null)
                    {
                        upClinic.Name = clinic.Name;
                        upClinic.Basis = clinic.Basis;
                        upClinic.Image = clinic.Image ?? "";
                        upClinic.Province = clinic.Province;
                        upClinic.Ward = clinic.Ward;
                        upClinic.District = clinic.District;
                        upClinic.Address = clinic.Address;
                        upClinic.Email = clinic.Email;
                        upClinic.PhoneNumber = clinic.PhoneNumber;
                        upClinic.Description = clinic.Description;
                    }
                    _context.Update(upClinic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClinicExists(clinic.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            //ViewData["ManagerID"] = new SelectList(_context.Accounts, "ID", "AccountStatus", clinic.ManagerID);
            //return View(clinic);
            List<string> errors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            string errorMessage = string.Join("\n", errors);
            return BadRequest(errorMessage);
        }

        // GET: Manager/Clinics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics
                .Include(c => c.Manager)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // POST: Manager/Clinics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic != null)
            {
                _context.Clinics.Remove(clinic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicExists(int id)
        {
            return _context.Clinics.Any(e => e.ID == id);
        }
    }
}
