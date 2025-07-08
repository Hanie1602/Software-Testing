using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.Areas.Manager.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Google.Apis.PeopleService.v1.Data;

namespace Dental_Clinic_System.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class SchedulesController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public SchedulesController(DentalClinicDbContext context)
        {
            _context = context;
        }

        // GET: Manager/Schedules
        public async Task<IActionResult> Index()
        {
            var dentalClinicDbContext = _context.Schedules.Include(s => s.Dentist).ThenInclude(d => d.Account).Include(s => s.TimeSlot);
            return View(await dentalClinicDbContext.ToListAsync());
        }
		// GET: Manager/LichLamViec
		public async Task<IActionResult> LichLamViec()
		{
            // Lấy các lịch làm việc của các nha sĩ thuộc phòng khám cụ thể - mà manager đang quản lý
            //Ví dụ phòng khám có ID=1 (Nha khoa đại dương)
            var den_sesList = _context.Dentist_Sessions.Include(d => d.Dentist).ThenInclude(a => a.Account).Include(d => d.Session).AsQueryable();
			den_sesList = den_sesList.Where(p => p.Dentist.ClinicID == 1); //Lấy tất cả các dòng của Dentist_Sessions where DentistIDs thuộc Clinic có id=1 
            //ViewBag.DenSesList = await den_SesList.ToListAsync(); ;
            return View(await den_sesList.ToListAsync());
		}
		// POST: Manager/LichLamViec
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LichLamViec(List<int> SelectedDenSesList)
        {
			// Lấy các lịch làm việc của các nha sĩ thuộc phòng khám cụ thể - mà manager đang quản lý
			//Ví dụ phòng khám có ID=1 (Nha khoa đại dương)
			var den_sesList = _context.Dentist_Sessions.Include(d => d.Dentist).ThenInclude(a => a.Account).Include(d => d.Session).AsQueryable();
			den_sesList = den_sesList.Where(p => p.Dentist.ClinicID == 1); //Lấy tất cả các dòng của Dentist_Sessions where DentistIDs thuộc Clinic có id=1 
            foreach (var denses in den_sesList)
            {
                if (SelectedDenSesList.Contains(denses.ID)) //nếu được chọn
                    denses.Check = true;
                else
                    denses.Check = false;
                _context.Update(denses);
            }
			await _context.SaveChangesAsync();
            //Lấy danh sách để in ra
			den_sesList = den_sesList.Where(p => p.Dentist.ClinicID == 1); //Lấy tất cả các dòng của Dentist_Sessions where DentistIDs thuộc Clinic có id=1 
			return View(await den_sesList.ToListAsync());
        }




		// GET: Manager/Schedules/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Dentist)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Manager/Schedules/Create
        public IActionResult Create()
        {
            var dentists = _context.Dentists
                           .Join(_context.Accounts,
                                 dentist => dentist.AccountID,
                                 account => account.ID,
                                 (dentist, account) => new
                                 {
                                     DentistID = dentist.ID,
                                     FullName = account.LastName + " " + account.FirstName
								 })
                           .ToList();

            ViewData["DentistID"] = new SelectList(dentists, "DentistID", "FullName");

            //ViewData["DentistID"] = new SelectList(_context.Dentists, "ID", "ID");
            //ViewData["TimeSlotID"] = new SelectList(_context.TimeSlots, "ID", "ID");
            return View();
        }

        // POST: Manager/Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,DentistID,TimeSlotID,Date,ScheduleStatus")] Schedule schedule)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(schedule);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DentistID"] = new SelectList(_context.Dentists, "ID", "ID", schedule.DentistID);
        //    ViewData["TimeSlotID"] = new SelectList(_context.TimeSlots, "ID", "ID", schedule.TimeSlotID);
        //    return View(schedule);
        //}
        public async Task<IActionResult> Create([Bind("DentistIDs, Dates, TimeSlots")] ScheduleVM schedule)
        {
			if (ModelState.IsValid)
            {
				List<DateOnly> dateList = ConvertStringToDateOnlyList(schedule.Dates);
				foreach (var dentist in schedule.DentistIDs)
                {
					foreach (var date in dateList)
					{
						foreach (var slot in schedule.TimeSlots)
						{
							var existSchedule = await _context.Schedules.FirstOrDefaultAsync(
								a => a.DentistID == dentist && a.Date == date && a.TimeSlotID == slot);
							if (existSchedule == null)
							{
								var newSchedule = new Schedule
								{
									//DentistID = schedule.DentistIDs,
                                    DentistID = dentist,
									Date = date,
									TimeSlotID = slot,
									ScheduleStatus = "Còn Trống"
								};
								_context.Add(newSchedule);
							}
						}
					}
				}
                
                //------------
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }
		private List<DateOnly> ConvertStringToDateOnlyList(string dateString)
		{
			List<DateOnly> dateList = new List<DateOnly>();
			string[] dateArray = dateString.Split(new[] { ", " }, StringSplitOptions.None);

			foreach (string date in dateArray)
			{
				DateOnly parsedDate = DateOnly.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
				dateList.Add(parsedDate);
			}

			return dateList;
		}

		// GET: Manager/Schedules/Edit/5
		public async Task<IActionResult> Edit(int? dentistId, DateTime? date)
        {
            if (dentistId == null || date == null)
            {
                return NotFound();
            }

            var scheduleSubList = _context.Schedules.Include(s => s.Dentist).ThenInclude(d => d.Account).Include(s => s.TimeSlot).AsQueryable();
            scheduleSubList = scheduleSubList.Where(p =>
                p.DentistID == dentistId && p.Date == DateOnly.FromDateTime(date.Value));

            var dentist = await _context.Dentists.Include(d => d.Account).FirstOrDefaultAsync(m => m.ID == dentistId);
			ViewBag.DentistName = dentist.Account.LastName + " " + dentist.Account.FirstName;
            ViewBag.Date = DateOnly.FromDateTime(date.Value);

			return View(await scheduleSubList.ToListAsync());
			
			//-----
			//var schedule = await _context.Schedules.FindAsync(dentistId);
   //         if (schedule == null)
   //         {
   //             return NotFound();
   //         }
   //         ViewData["DentistID"] = new SelectList(_context.Dentists, "ID", "ID", schedule.DentistID);
   //         ViewData["TimeSlotID"] = new SelectList(_context.TimeSlots, "ID", "ID", schedule.TimeSlotID);
   //         return View(schedule);
        }

        // POST: Manager/Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DentistID,TimeSlotID,Date,ScheduleStatus")] Schedule schedule)
        {
            if (id != schedule.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DentistID"] = new SelectList(_context.Dentists, "ID", "ID", schedule.DentistID);
            ViewData["TimeSlotID"] = new SelectList(_context.TimeSlots, "ID", "ID", schedule.TimeSlotID);
            return View(schedule);
        }

        // GET: Manager/Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Dentist).ThenInclude(d => d.Account)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Manager/Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules
				.Include(s => s.Dentist).ThenInclude(d => d.Account)
				.Include(s => s.TimeSlot)
				.FirstOrDefaultAsync(m => m.ID == id);
			var denId = schedule.Dentist.ID;
            var date = schedule.Date;
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { dentistId = denId, date = date });
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.ID == id);
        }
    }
}
