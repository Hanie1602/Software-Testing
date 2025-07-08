using Azure;
using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.Services.MOMO;
using Dental_Clinic_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Dental_Clinic_System.Areas.Dentist.Controllers
{
    [Area("dentist")]
    [Authorize(AuthenticationSchemes = "DentistScheme", Roles = "Nha Sĩ")]
    public class DentistDetailController : Controller
    {
        private readonly DentalClinicDbContext _context;
        private readonly IMOMOPayment _momoPayment;
        public DentistDetailController(DentalClinicDbContext context, IMOMOPayment momoPayment)
        {
            _context = context;
            _momoPayment = momoPayment;
        }

        public IActionResult Index()
        {
            TempData.Keep("userID");
            return RedirectToAction("DentistSchedule");
        }



        #region Lấy lịch làm việc của Dentist, đưa vào Calendar
        [HttpGet]
        //[Authorize(Roles = "Nha Sĩ")]
        public async Task<IActionResult> DentistSchedule()
        {

            var dentistAccountID = HttpContext.Session.GetInt32("dentistAccountID");
            if (dentistAccountID == null)
            {
                return RedirectToAction("Login", "DentistAccount", new { area = "Dentist" });
            }

            // Lấy Schedule của dentist cụ thể
            var schedules = await _context.Schedules
                                    .Include(s => s.Dentist)
                                    .Include(s => s.TimeSlot)
                                    .Where(s => s.Dentist.Account.ID == dentistAccountID)
                                    .ToListAsync();

            if (schedules == null || !schedules.Any())
            {
                ViewBag.message = "Không tìm thấy lịch phù hợp";
                return View();
            }

            // Map 2 bên Schedule với appointment, sau đó map những cột appointment ko null với patient record
            var scheduleAppointments = from schedule in schedules
                                       join appointment in _context.Appointments
                                       on schedule.ID equals appointment.ScheduleID into appointmentGroup
                                       from appointment in appointmentGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           appointmentID = appointment?.ID ?? 0,
                                           scheduleDate = schedule.Date,
                                           patientID = appointment?.PatientRecordID ?? 0,
                                           startTime = schedule?.TimeSlot?.StartTime,
                                           endTime = schedule?.TimeSlot?.EndTime,
                                           patientRecordID = appointment?.PatientRecordID ?? 0
                                       };


            var result = from sa in scheduleAppointments
                         join patientRecord in _context.PatientRecords
                         on sa.patientRecordID equals patientRecord.ID into patientGroup
                         from patientRecord in patientGroup.DefaultIfEmpty()
                         select new
                         {
                             sa.appointmentID,
                             sa.scheduleDate,
                             sa.startTime,
                             sa.endTime,
                             patientName = patientRecord?.FullName ?? "No Patient"
                         };

            // Map to EventVM
            var events = result.Select(s => new EventVM
            {
                Title = s.appointmentID != 0 ? $"#{s.appointmentID} - {s.patientName}" : "Trống",
                Start = s.scheduleDate != null && s.startTime.HasValue ? $"{s.scheduleDate:yyyy-MM-dd}T{s.startTime.Value:HH:mm:ss}" : null,
                End = s.scheduleDate != null && s.endTime.HasValue ? $"{s.scheduleDate:yyyy-MM-dd}T{s.endTime.Value:HH:mm:ss}" : null,
            }).ToList();


            // Lấy thông tin Nha sĩ
            var dentist = await _context.Dentists
                                        .Include(d => d.Account)
                                        .FirstOrDefaultAsync(d => d.Account.ID == dentistAccountID);

            // Gửi thông tin qua View
            ViewBag.dentistAvatar = dentist.Account.Image;
            ViewBag.dentistName = $"{dentist.Account.LastName} {dentist.Account.FirstName}";
            ViewBag.events = JsonConvert.SerializeObject(events);
            return View();
        }

        #endregion

        #region Chỉnh sửa thông tin của Dentist

        [HttpGet]
        public async Task<IActionResult> DentistDescription()
        {
            var dentistAccountID = HttpContext.Session.GetInt32("dentistAccountID");
            if (dentistAccountID == null)
            {
                return RedirectToAction("Login", "DentistAccount", new { area = "Dentist" });
            }
            var dentist = await _context.Dentists.Where(d => d.Account.ID == dentistAccountID).Include(d => d.Account).FirstAsync();
            ViewBag.DentistAvatar = dentist?.Account.Image;
            ViewBag.DentistName = dentist?.Account.LastName + " " + dentist?.Account.FirstName;
            var message = TempData["Message"] as string;
            ViewBag.message = message;
            return View("DentistDescription", dentist);
        }





        [HttpPost]
        public async Task<IActionResult> SendDentistDescription(string? content)
        {
            var dentistAccountID = HttpContext.Session.GetInt32("dentistAccountID");
            if (dentistAccountID == null)
            {
                return RedirectToAction("Login", "DentistAccount", new { area = "Dentist" });
            }
            if (string.IsNullOrEmpty(content))
            {
                TempData["Message"] = "Lỗi! Mô tả không được để trống.";
                return RedirectToAction("DentistDescription");
            }
            var dentist = await _context.Dentists.FirstOrDefaultAsync(d => d.AccountID == dentistAccountID);
            dentist.Description = content;
            _context.Dentists.Update(dentist);
            await _context.SaveChangesAsync();

            TempData["Message"] = "success";

            return RedirectToAction("DentistDescription");
        }

        #endregion

        #region Quản lý lịch đặt khám của bệnh nhân (Bản thử nghiệm và những dữ liệu quan trọng nữa)

        [HttpGet]
        public async Task<IActionResult> PatientAppointments()
        {
            var dentistAccountID = HttpContext.Session.GetInt32("dentistAccountID");
            if (dentistAccountID == null)
            {
                return RedirectToAction("Login", "DentistAccount", new { area = "Dentist" });
            }

            var dentist = await _context.Dentists.Where(d => d.Account.ID == dentistAccountID).Include(d => d.Account).FirstAsync();
            var appointments = await _context.Appointments
                                    .Include(a => a.Schedule).ThenInclude(s => s.TimeSlot)
                                    .Include(a => a.PatientRecords)
                                    .Include(a => a.Specialty)
                                    .Where(a => a.Schedule.DentistID == dentist.ID)
                                    .ToListAsync();
            ViewBag.DentistAvatar = dentist?.Account.Image;
            ViewBag.DentistName = dentist?.Account.LastName + " " + dentist?.Account.FirstName;

            ViewBag.Message = TempData["Message"];
            return View("PatientAppointments", appointments);
        }




        [HttpPost]
        public async Task<IActionResult> ChangePatientAppointment(int appointmentID, string appointmentStatus)
        {
            var dentistAccountID = HttpContext.Session.GetInt32("dentistAccountID");
            if (dentistAccountID == null)
            {
                return RedirectToAction("Login", "DentistAccount", new { area = "Dentist" });
            }
            var appointment = await _context.Appointments.Include(a => a.Transactions).Where(a => a.ID == appointmentID).FirstOrDefaultAsync();
            appointment.AppointmentStatus = appointmentStatus;

            _context.Update(appointment);
            await _context.SaveChangesAsync();

            var schedule = await _context.Appointments.Include(s => s.Schedule).FirstOrDefaultAsync(a => a.ID == appointmentID);
            if(appointment.AppointmentStatus == "Đã Khám" || appointment.AppointmentStatus == "Đã Hủy")
            {
                schedule.Schedule.ScheduleStatus = "Còn Trống";
                await _context.SaveChangesAsync();
            }

            // HERE
            #region Refund MOMO API
            if (appointment.AppointmentStatus == "Đã Khám")
            {
                // Invoke the refund method from payment controller
                // Hoàn tiền thành công
                //return View("RefundSuccess", responseObject);
                var transactionCode = appointment.Transactions.FirstOrDefault()?.TransactionCode;
                var amount = appointment.Transactions.FirstOrDefault()?.TotalPrice;
                var bankName = appointment.Transactions.FirstOrDefault()?.BankName;
                var fullName = appointment.Transactions.FirstOrDefault()?.FullName;
                if (_momoPayment.RefundPayment((long)decimal.Parse(amount.ToString()), long.Parse(transactionCode.ToString()), "") != null)
                {
                    TempData["RefundMessage"] = "Hoàn tiền thành công";

                    var transaction = new Transaction
                    {
                        AppointmentID = appointment.ID,
                        Date = DateTime.Now,
                        BankName = bankName,
                        TransactionCode = transactionCode,
                        PaymentMethod = "MOMO",
                        TotalPrice = amount,
                        BankAccountNumber = "9704198526191432198",
                        FullName = fullName,
                        Message = "Hoàn tiền thành công",
                        Status = "Thành Công"
                    };

                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();
                }

            }
            #endregion

            TempData["message"] = "success";
            return RedirectToAction("PatientAppointments");
        }
        #endregion

        //Hàm hủy đơn đặt
        public async Task<IActionResult> CancelAppointment(int appointmentID)  //string description
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.ID == appointmentID && (a.AppointmentStatus == "Đã Chấp Nhận" || a.AppointmentStatus == "Chờ Xác Nhận"));
            if(appointment == null)
            {
				ViewBag.message = "Lỗi! Không tìm thấy đơn đặt tương ứng hoặc trạng thái không hợp lệ.";
				return RedirectToAction("patientappointments");
			}
            appointment.AppointmentStatus = "Đã Hủy";
            //appointment.Description = "Lý do hủy: " + description;
            _context.Update(appointment);
           await _context.SaveChangesAsync();
			ViewBag.message = "success";
			return RedirectToAction("patientappointments");
        }

        //Hàm thay đổi trạng thái của đơn đặt
		public async Task<IActionResult> ChangeStatusAppointment(int appointmentID, int statusNumber)
		{
			var appointment = _context.Appointments.FirstOrDefault(a => a.ID == appointmentID && (a.AppointmentStatus == "Chờ Xác Nhận" || a.AppointmentStatus == "Đã Chấp Nhận"));
			if (appointment == null)
			{
				ViewBag.message = "Lỗi! Không tìm thấy đơn đặt tương ứng hoặc trạng thái không hợp lệ.";
				return NotFound("patientappointments");
			}

            if(statusNumber == 1)
            {
				appointment.AppointmentStatus = "Đã Chấp Nhận";
			}
            else if(statusNumber == 2)
			{
				appointment.AppointmentStatus = "Đã Khám";
			}
			_context.Update(appointment);
			await _context.SaveChangesAsync();
			ViewBag.message = "success";
			return RedirectToAction("patientappointments");
		}

	}
}
