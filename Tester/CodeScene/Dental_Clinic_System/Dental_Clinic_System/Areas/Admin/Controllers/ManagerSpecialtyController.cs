using Dental_Clinic_System.Areas.Admin.Models;
using Dental_Clinic_System.Areas.Admin.ViewModels;
using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;

namespace Dental_Clinic_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin/[controller]")]
    [Authorize(AuthenticationSchemes = "GetAppointmentStatus", Roles = "Admin")]
    public class ManagerSpecialtyController : Controller
    {
        private readonly DentalClinicDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HiddenSpecialtyService _hiddenSpecialty;

        public ManagerSpecialtyController(DentalClinicDbContext context, HiddenSpecialtyService hiddenSpecialty, IWebHostEnvironment iwebhostenvironment)
        {
            _context = context;
            _hiddenSpecialty = hiddenSpecialty;
            _webHostEnvironment = iwebhostenvironment;
        }

        #region Show List Chuyên Khoa
        //===================LIST CHUYÊN KHOA===================
        //[Route("ListSpecialty")]
        public async Task<IActionResult> ListSpecialty()
        {
            var hiddenSpecialties = _hiddenSpecialty.GetHiddenSpecialty();

            var specialty = await _context.Specialties
                .Where(s => !hiddenSpecialties.Contains(s.ID))
                .ToListAsync();

            var specialtyList = specialty.Select(s => new ManagerSpecialtyVM
            {
                Id = s.ID,
                Name = s.Name,
                Image = s.Image,
                Description = s.Description,
                Deposit = s.Deposit
            }).ToList();

            return View("ListSpecialty", specialtyList);
        }
        #endregion

        #region Tìm kiếm (Search)
        //===================TÌM KIẾM===================
        //[Route("SeachSpecialty")]
        public async Task<IActionResult> SeachSpecialty(string search)
        {
            var hiddenSpecialties = _hiddenSpecialty.GetHiddenSpecialty();

            var specialty = await _context.Specialties
                .Where(s => !hiddenSpecialties.Contains(s.ID) && (string.IsNullOrEmpty(search) || s.Name.Contains(search)))
                .ToListAsync();

            var specialtyList = specialty.Select(s => new ManagerSpecialtyVM
            {
                Id = s.ID,
                Name = s.Name,
                Image = s.Image,
                Description = s.Description,
                Deposit = s.Deposit
            }).ToList();

            return View(nameof(ListSpecialty), specialtyList);
        }
        #endregion

        #region Chỉnh sửa (Edit)
        //===================CHỈNH SỬA===================
        [HttpGet]
        //[Route("EditSpecialty/{id}")]
        public async Task<IActionResult> EditSpecialty(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }

            var viewModel = new ManagerSpecialtyVM
            {
                Id = specialty.ID,
                Name = specialty.Name,
                Image = specialty.Image,
                Description = specialty.Description,
                Deposit = specialty.Deposit
            };

            return View(viewModel);
        }

        [HttpPost]
        //[Route("EditSpecialty")]
        public async Task<IActionResult> EditSpecialty(ManagerSpecialtyVM model)
        {
            if (ModelState.IsValid)
            {
                var specialty = await _context.Specialties.FindAsync(model.Id);

                if (specialty == null)
                {
                    return NotFound();
                }

                // Kiểm tra tên chuyên khoa có bị trùng không
                var isDuplicateName = await _context.Specialties
                    .FirstOrDefaultAsync(s => s.Name == model.Name);
                if (isDuplicateName != null)
                {
                    ModelState.AddModelError("Name", "Tên chuyên khoa đã tồn tại.");
                    return View("EditSpecialty", model);
                }

                specialty.Name = model.Name;
                specialty.Description = model.Description;
                specialty.Deposit = model.Deposit;
                specialty.Image = model.Image;

                _context.Specialties.Update(specialty);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListSpecialty));
            }

            return View("EditSpecialty", model);
        }
        #endregion

        #region Xóa tạm thời (Delete)
        //===================XÓA CHUYÊN KHOA TẠM THỜI===================
        //[Route("DeleteSpecialty/{id}")]
        public IActionResult DeleteSpecialty(int id)
        {
            _hiddenSpecialty.HiddenSpecialty(id);
            return RedirectToAction(nameof(ListSpecialty));
        }
        #endregion

        #region Thêm chuyên khoa (Add)
        //===================THÊM CHUYÊN KHOA===================
        [HttpPost]
        //[Route("AddSpecialty")]
        public async Task<IActionResult> AddSpecialty(string name, string description, string image, decimal deposit)
        {
            //Check Tên chuyên khoa có bị trùng không
            var existingName = await _context.Specialties.FirstOrDefaultAsync(s => s.Name == name);

            if (existingName != null)
            {
                //Thấy tên bị trùng, thông báo lỗi
                ModelState.AddModelError("Name", "Tên chuyên khoa đã tồn tại. Vui lòng chọn tên khác.");

                //Lấy lại list specialty để hiển thị
                var specialties = await _context.Specialties.ToListAsync();

                var listSpecialty = specialties.Select(s => new ManagerSpecialtyVM
                {
                    Id = s.ID,
                    Name = s.Name,
                    Image = s.Image,
                    Description = s.Description,
                    Deposit = s.Deposit
                }).ToList();

                return View(nameof(ListSpecialty), listSpecialty);
            }

            //Check Deposis không được âm
            if (deposit <= 0)
            {
                //Thấy Tiền cọc âm, thông báo lỗi
                ModelState.AddModelError("Deposit", "Tiền cọc phải lớn hơn 0!!!");

                //Lấy lại list specialty để hiển thị
                var specialties = await _context.Specialties.ToListAsync();
                var specialtyList = specialties.Select(s => new ManagerSpecialtyVM
                {
                    Id = s.ID,
                    Name = s.Name,
                    Image = s.Image,
                    Description = s.Description,
                    Deposit = s.Deposit
                }).ToList();

                return View(nameof(ListSpecialty), specialtyList);
            }

            var specialty = new Specialty
            {
                Name = name,
                Description = description,
                Image = image,
                Deposit = deposit
            };

            _context.Specialties.Add(specialty);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListSpecialty));
        }
        #endregion

        #region Xem thông tin (View)
        //[Route("ViewSpecialty")]
        public async Task<IActionResult> ViewSpecialty(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }

            var specialtyVM = new ManagerSpecialtyVM
            {
                Id = specialty.ID,
                Name = specialty.Name,
                Deposit = specialty.Deposit,
                Description = specialty.Description,
                Image = specialty.Image
            };

            return Json(specialtyVM);
        }
        #endregion
    }
}


//KIẾM TRA LỖI
//List<string> errors = new List<string>();
//foreach (var value in ModelState.Values)
//{
//	foreach (var error in value.Errors)
//	{
//		errors.Add(error.ErrorMessage);
//	}
//}
//string errorMessage = string.Join("\n", errors);
//return BadRequest(errorMessage);