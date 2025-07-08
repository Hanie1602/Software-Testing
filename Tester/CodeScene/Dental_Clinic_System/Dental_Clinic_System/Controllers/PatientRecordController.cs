using Dental_Clinic_System.Areas.Manager.ViewModels;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.ViewModels;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Security.Policy;


namespace Dental_Clinic_System.Controllers
{
    [Authorize(Roles = "Bệnh Nhân")]
    public class PatientRecordController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public PatientRecordController(DentalClinicDbContext context)
        {
            _context = context;
        }

        //=============================== PATIENT RECORD - CLINIC PART =========================================
        #region Hàm lấy tất cả patient record có liên quan đến user hiện tại
        [HttpGet]
        public async Task<IActionResult> PatientRecord(int clinicID, int specialtyID, int dentistID, string scheduleID)
        {
            var claimsEmailValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var username = claimsEmailValue;
            var patientRecord = await _context.PatientRecords
                                        .Include(pr => pr.Account)
                                        .Include(pr => pr.Appointments)
                                        .Where(pr => pr.Account.Email == username && pr.PatientRecordStatus == "Đang Tồn Tại")
                                        .ToListAsync();
            TempData["Patient Amount"] = patientRecord.Count;
            ViewBag.scheduleID = scheduleID;
            ViewBag.specialtyID = specialtyID;
            ViewBag.dentistID = dentistID;
            ViewBag.clinicID = clinicID;
            return View(patientRecord);
        }
        #endregion

        #region Hàm này chỉ show ra View tạo mới patient record
        [HttpGet]
        [Route("/createnewpatientrecord")]
        public async Task<IActionResult> ShowFormCreatingNewPatientRecord()
        {
            //ViewBag.returnUrl = returnUrl;
            return View("createnewpatientrecord");
        }
        #endregion

        #region Hàm nhận dữ liệu hồ sơ bệnh nhân (patient record) nếu chưa có hoặc tạo thêm hồ sơ bệnh nhân (đã release)
        [HttpPost]
        public async Task<IActionResult> CreateNewPatientRecord(PatientRecordVM patient)
        {
            var claimsEmailValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Accounts.FirstOrDefault(u => u.Email == claimsEmailValue);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng!!");
            }
            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = DateTime.Now.Year - patient.DateOfBirdth.Year;
            if (patient.DateOfBirdth > today.AddYears(-age)) age--;

            //Phần check xem đã  nhập Tỉnh thành, quận huyện, phường xã hay chưa
            if (patient.Province == 0)
            {
                ModelState.AddModelError("Province", "Yêu cầu nhập tỉnh / thành!");
                return View(patient);
            }
            if (patient.District == 0)
            {
                ModelState.AddModelError("District", "Yêu cầu nhập quận / huyện!");
                return View(patient);
            }
            if (patient.Ward == 0)
            {
                ModelState.AddModelError("Ward", "Yêu cầu nhập phường / xã!");
                return View(patient);
            }

            //Check theo độ tuổi
            if (age >= 14)
            {
                patient.FMFullName = null;
                patient.FMRelationship = null;
                patient.FMEmail = null;
                patient.FMPhoneNumber = null;
            }
            else
            {
                ModelState.Remove(nameof(patient.PhoneNumber));
                ModelState.Remove(nameof(patient.Job));
                ModelState.Remove(nameof(patient.IdentifyNumber));

                if (string.IsNullOrEmpty(patient.FMFullName))
                {
                    ModelState.AddModelError("FMFullName", "FMFullName is required.");
                    return View(patient);
                }

                if (string.IsNullOrEmpty(patient.FMRelationship))
                {
                    ModelState.AddModelError("FMRelationship", "FMRelationship is required.");
                    return View(patient);
                }

                if (string.IsNullOrEmpty(patient.FMEmail))
                {
                    ModelState.AddModelError("FMEmail", "FMEmail is required.");
                    return View(patient);
                }

                if (string.IsNullOrEmpty(patient.FMPhoneNumber))
                {
                    ModelState.AddModelError("FMPhoneNumber", "FMPhoneNumber is required.");
                    return View(patient);
                }
                patient.Job = "Còn nhỏ";
            }

            if (ModelState.IsValid)
            {
                var patientRecord = new PatientRecord()
                {
                    FullName = patient.FullName,
                    AccountID = user.ID,
                    DateOfBirth = patient.DateOfBirdth,
                    PhoneNumber = patient.PhoneNumber,
                    Gender = patient.Gender,
                    Job = patient.Job,
                    IdentityNumber = patient.IdentifyNumber,
                    EmailReceiver = patient.Email,
                    Province = patient.Province,
                    District = patient.District,
                    Ward = patient.Ward,
                    Address = patient.Address,
                    PatientRecordStatus = "Đang Tồn Tại",
                    //===== Family Member Part ====
                    FMName = patient.FMFullName,
                    FMEmail = patient.FMEmail,
                    FMPhoneNumber = patient.FMPhoneNumber,
                    FMRelationship = patient.FMRelationship

                };
                await _context.PatientRecords.AddAsync(patientRecord);
                await _context.SaveChangesAsync();
                //if (!string.IsNullOrEmpty(returnUrl))
                //{
                //    return Redirect(returnUrl);
                //}
                return RedirectToAction("PatientRecordInProfile");
            }

            return View(patient); // Trả lại view với các lỗi validation nếu có
        }
        #endregion

        #region Hàm hiện ra thông tin xác nhận tất cả thông tin về clinic, dentist và patient record
        [HttpGet]
        [Route("/appointment/confirm")]
        public async Task<IActionResult> ConfirmAppointment(int scheduleID, int patientRecordID, int specialtyID, int clinicID, int dentistID)
        {
            var appointment = new Dictionary<string, object>
            {
                { "Schedule", await _context.Schedules.Include(s => s.TimeSlot).FirstOrDefaultAsync(s => s.ID == scheduleID)},
                { "PatientRecord", await _context.PatientRecords.FirstOrDefaultAsync(pr => pr.ID == patientRecordID)},
                { "Specialty", await _context.Specialties.FirstOrDefaultAsync(sp => sp.ID == specialtyID)},
                { "Dentist", await _context.Dentists.Include(d => d.Account). FirstOrDefaultAsync(d => d.ID == dentistID)},
                { "Clinic", await _context.Clinics. FirstOrDefaultAsync(c => c.ID == clinicID) }
            };

            ViewBag.scheduleID = scheduleID;
            ViewBag.patientRecordID = patientRecordID;
            ViewBag.specialtyID = specialtyID;
            ViewBag.clinicID = clinicID;
            ViewBag.dentistID = dentistID;
            return View(appointment);
        }
        #endregion

        #region Hàm lấy thông tin liên quan đến việc tạo appointment, show ra và chọn phương thức thanh toán
        [HttpGet]
        public async Task<IActionResult> PatientRecordPaymentChoosing(int scheduleID, int patientRecordID, int specialtyID, int clinicID, int dentistID)
        {
            var appointment = new Dictionary<string, object>
            {
                { "Schedule", await _context.Schedules.Include(s => s.TimeSlot).FirstOrDefaultAsync(s => s.ID == scheduleID)},
                { "PatientRecord", await _context.PatientRecords.FirstOrDefaultAsync(pr => pr.ID == patientRecordID)},
                { "Specialty", await _context.Specialties.FirstOrDefaultAsync(sp => sp.ID == specialtyID)},
                { "Dentist", await _context.Dentists.Include(d => d.Account). FirstOrDefaultAsync(d => d.ID == dentistID)},
                { "Clinic", await _context.Clinics. FirstOrDefaultAsync(c => c.ID == clinicID) }
            };
            ViewBag.scheduleID = scheduleID;
            ViewBag.patientRecordID = patientRecordID;
            ViewBag.specialtyID = specialtyID;
            ViewBag.dentistID = dentistID;
            ViewBag.clinicID = clinicID;
            return View(appointment);
        }
        #endregion

        //============================== PATIENT RECORD - PROFILE PART ====================================
        #region Hàm lấy thông tin patient record từ account
        [HttpGet]
        public async Task<IActionResult> PatientRecordInProfile()
        {
            var claimsEmailValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Accounts.FirstOrDefault(u => u.Email == claimsEmailValue);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng hợp lệ");
            }
            var patientRecord = _context.PatientRecords.Where(pr => pr.AccountID == user.ID && pr.PatientRecordStatus == "Đang Tồn Tại").ToList();
            //if(patientRecord == null)
            //{
            //	return NotFound("Không tìm thấy dữ liệu hồ sơ tương ứng!");
            //}

            return View(patientRecord);
        }
        #endregion

        #region Hàm Set lại Status của patient record cụ thể
        public async Task<IActionResult> DeletePatientRecord(int patientRecordID)
        {
            var patientRecord = await _context.PatientRecords.FirstOrDefaultAsync(pr => pr.ID == patientRecordID);
            if (patientRecord == null)
            {
                TempData["Message"] = "Xóa thất bại, không tìm thấy hồ sơ tương ứng!";
                return RedirectToAction("PatientRecordInProfile");
            }
            patientRecord.PatientRecordStatus = "Đã Xóa";
            _context.PatientRecords.Update(patientRecord);
            await _context.SaveChangesAsync();
            TempData["Message"] = "success";
            return RedirectToAction("PatientRecordInProfile");
        }
        #endregion

        #region Show ra thông tin của patient record cần thay đổi
        [HttpGet]
        public async Task<IActionResult> EditPatientRecord(int patientRecordID)
        {
            var claimsEmailValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Accounts.FirstOrDefault(u => u.Email == claimsEmailValue);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng hợp lệ");
            }
            var patientRecord = _context.PatientRecords.FirstOrDefault(pr => pr.ID == patientRecordID);
            if (patientRecord == null)
            {
                return NotFound("Không tìm thấy hồ sơ cụ thể!");
            }
            //============= RECORD PART=================
            PatientRecordVM patientRecordVM;
            int age = DateTime.Now.Year - patientRecord.DateOfBirth.Year;
            patientRecordVM = new()
            {
                FullName = patientRecord.FullName,
                DateOfBirdth = patientRecord.DateOfBirth,
                PhoneNumber = patientRecord.PhoneNumber,
                Gender = patientRecord.Gender,
                Job = patientRecord.Job,
                IdentifyNumber = patientRecord.IdentityNumber,
                Email = patientRecord.EmailReceiver,
                Province = patientRecord.Province,
                District = patientRecord.District,
                Ward = patientRecord.Ward,
                Address = patientRecord.Address,
                //===== FM Part =====
                FMFullName = patientRecord.FMName,
                FMEmail = patientRecord.FMEmail,
                FMRelationship = patientRecord.FMRelationship,
                FMPhoneNumber = patientRecord.FMPhoneNumber
            };
            return View(patientRecordVM);
        }
        #endregion

        #region Thay đổi thông tin patient record
        [HttpPost]
        public async Task<ActionResult> EditPatientRecord(int patientRecordID, PatientRecordVM patient)
        {
            var patientRecord = await _context.PatientRecords.FirstOrDefaultAsync(pr => pr.ID == patientRecordID && pr.PatientRecordStatus == "Đang Tồn Tại");
            if (patientRecord == null)
            {
                return NotFound("Không tìm thấy hồ sơ");
            }
            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = DateTime.Now.Year - patient.DateOfBirdth.Year;
            if (patient.DateOfBirdth > today.AddYears(-age)) age--;

            //Phần check xem đã  nhập Tỉnh thành, quận huyện, phường xã hay chưa
            if (patient.Province == 0)
            {
                ModelState.AddModelError("Province", "Yêu cầu nhập tỉnh / thành!");
                return View(patient);
            }
            if (patient.District == 0)
            {
                ModelState.AddModelError("District", "Yêu cầu nhập quận / huyện!");
                return View(patient);
            }
            if (patient.Ward == 0)
            {
                ModelState.AddModelError("Ward", "Yêu cầu nhập phường / xã!");
                return View(patient);
            }

            //Check theo độ tuổi
            if (age < 14)
            {
                ModelState.Remove(nameof(patient.PhoneNumber));
                ModelState.Remove(nameof(patient.Job));
                ModelState.Remove(nameof(patient.IdentifyNumber));

                if (string.IsNullOrEmpty(patient.FMFullName))
                {
                    ModelState.AddModelError("FMFullName", "FMFullName is required.");
                    return View(patient);
                }

                if (string.IsNullOrEmpty(patient.FMRelationship))
                {
                    ModelState.AddModelError("FMRelationship", "FMRelationship is required.");
                    return View(patient);
                }

                if (string.IsNullOrEmpty(patient.FMEmail))
                {
                    ModelState.AddModelError("FMEmail", "FMEmail is required.");
                    return View(patient);
                }

                if (string.IsNullOrEmpty(patient.FMPhoneNumber))
                {
                    ModelState.AddModelError("FMPhoneNumber", "FMPhoneNumber is required.");
                    return View(patient);
                }

                patient.Job = "Còn nhỏ";
            }
            else
            {
                // Xóa family member info nếu age > 14 
                patient.FMFullName = null;
                patient.FMRelationship = null;
                patient.FMEmail = null;
                patient.FMPhoneNumber = null;
            }

            if (ModelState.IsValid)
            {
                patientRecord.FullName = patient.FullName;
                patientRecord.DateOfBirth = patient.DateOfBirdth;
                patientRecord.PhoneNumber = patient.PhoneNumber;
                patientRecord.Gender = patient.Gender;
                patientRecord.Job = patient.Job;
                patientRecord.IdentityNumber = patient.IdentifyNumber;
                patientRecord.EmailReceiver = patient.Email;
                patientRecord.Province = patient.Province;
                patientRecord.District = patient.District;
                patientRecord.Ward = patient.Ward;
                patientRecord.Address = patient.Address;
                //===== Family Member Part ====
                patientRecord.FMName = patient.FMFullName;
                patientRecord.FMEmail = patient.FMEmail;
                patientRecord.FMPhoneNumber = patient.FMPhoneNumber;
                patientRecord.FMRelationship = patient.FMRelationship;

                if (age >= 14)
                {
                    patientRecord.FMName = null;
                    patientRecord.FMEmail = null;
                    patientRecord.FMPhoneNumber = null;
                    patientRecord.FMRelationship = null;
                }


                _context.PatientRecords.Update(patientRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction("PatientRecordInProfile");
            }
            return View(patient);
        }
        #endregion

    }
}