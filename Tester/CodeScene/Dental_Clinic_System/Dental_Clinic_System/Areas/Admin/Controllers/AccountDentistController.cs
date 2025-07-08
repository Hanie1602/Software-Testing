//==============================================TÀI KHOẢN NHA SĨ================================================
using AutoMapper;
using Dental_Clinic_System.Areas.Admin.Models;
using Dental_Clinic_System.Areas.Admin.ViewModels;
using Dental_Clinic_System.Areas.Manager.ViewModels;
using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;

namespace Dental_Clinic_System.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(AuthenticationSchemes = "GetAppointmentStatus", Roles = "Admin")]

	public class AccountDentistController : Controller
	{
		private readonly DentalClinicDbContext _context;

		public AccountDentistController(DentalClinicDbContext context)
		{
			_context = context;
		}

		#region Show List Account với Role Nha Sĩ
		//===================LIST ACCOUNT===================
		//[Route("ListAccountDentist")]
		public async Task<IActionResult> ListAccountDentist()
		{
			var accountsQuery = _context.Accounts
			.Where(a => a.AccountStatus == "Hoạt Động" && a.Role == "Nha Sĩ");

			var accounts = await accountsQuery.ToListAsync();

			var accountList = accounts.Select(a => new ManagerAccountVM
			{
				Id = a.ID,
				Username = a.Username,
				Email = a.Email,
				PhoneNumber = a.PhoneNumber,
				Address = a.Address,
				Gender = a.Gender,
				Role = a.Role,

				//Status = a.AccountStatus
			}).ToList();


			return View(accountList);
		}
		#endregion

		#region Tìm kiếm (Search)
		//===================TÌM KIẾM===================
		//[Route("SearchAccount")]
		public async Task<IActionResult> SearchAccount(string keyword)
		{
			//Nếu keyword rỗng, sẽ chuyển hướng đến ListAccount
			if (string.IsNullOrEmpty(keyword))
			{
				return RedirectToAction(nameof(ListAccountDentist));
			}

			var accountQuesry = _context.Accounts
			.Where(a => a.Username.Contains(keyword) && a.AccountStatus == "Hoạt Động" && a.Role == "Nha Sĩ");

			var account = await accountQuesry.Where(a => a.Username.Contains(keyword)).ToListAsync();

			var accountList = account.Select(a => new ManagerAccountVM
			{
				Id = a.ID,
				Username = a.Username,
				Email = a.Email,
				PhoneNumber = a.PhoneNumber,
				Address = a.Address,
				Gender = a.Gender,
				Role = a.Role
				//Status = a.AccountStatus
			}).ToList();

			return View("ListAccountDentist", accountList);
		}
		#endregion

		#region Thêm tài khoản (Add Account)
		//[Route("GetDegrees")]
		[HttpGet]
		public async Task<IActionResult> GetDegrees()
		{
			var degrees = await _context.Degrees.ToListAsync();
			return Json(degrees);
		}

		//[Route("GetClinics")]
		[HttpGet]
		public async Task<IActionResult> GetClinics()
		{
			var clinic = await _context.Clinics.ToListAsync();
			return Json(clinic);
		}

		//===================THÊM TÀI KHOẢN===================
		//[Route("AddAccount")]
		[HttpPost]
		public async Task<IActionResult> AddAccount(string username, string password, string phoneNumber, string email, string gender, string firstname, string lastname, int degreeID, int clinicID)
		{
			//Check thông tin trùng lặp
			var existingAccount = await _context.Accounts
				.FirstOrDefaultAsync(a => a.Email == email || a.PhoneNumber == phoneNumber || a.Username == username);

			if (existingAccount != null)
			{
				//Thấy thông tin bị trùng, thông báo lỗi
				ModelState.AddModelError(string.Empty, "Thông tin người dùng đã tồn tại.");

				//Lấy lại list account để hiển thị
				var account1 = await _context.Accounts
					.Where(a => a.AccountStatus == "Hoạt Động" && a.Role == "Nha Sĩ")
					.ToListAsync();

				var accountList = account1.Select(a => new ManagerAccountVM
				{
					Id = a.ID,
					FirstName = a.FirstName,
					LastName = a.LastName,
					Username = a.Username,
					Email = a.Email,
					Gender = a.Gender,
					PhoneNumber = a.PhoneNumber,
					Role = a.Role
				}).ToList();

				return View("ListAccountDentist", accountList);
			}

			//Thêm mới Account vào DB
			var account = new Account
			{
				Username = username,
				FirstName = firstname,
				LastName = lastname,
				Password = DataEncryptionExtensions.ToMd5Hash(password),
				PhoneNumber = phoneNumber,
				Email = email,
				Gender = gender,
				Role = "Nha Sĩ",
				AccountStatus = "Hoạt Động"
			};
			_context.Accounts.Add(account);
			await _context.SaveChangesAsync();

			//Thêm mới Dentist vào DB
			var newDentist = new Dental_Clinic_System.Models.Data.Dentist
			{
				AccountID = account.ID,
				ClinicID = clinicID,
				DegreeID = degreeID,
				//Description = accountDentist.Description
			};
			_context.Dentists.Add(newDentist);
			await _context.SaveChangesAsync();

			var dentists = await _context.Dentists.Include(d => d.Account).Include(d => d.Clinic).Include(d => d.Degree).ToListAsync();

			return RedirectToAction(nameof(ListAccountDentist), dentists);
		}

		#endregion

		#region Chỉnh sửa (Edit Account)
		//===================CHỈNH SỬA TÀI KHOẢN===================

		//[Route("EditAccount/{id}")]
		[HttpGet]
		public async Task<IActionResult> EditAccountDentist(int id)
		{
			var account = await _context.Accounts.FindAsync(id);
			if (account == null)
			{
				Console.WriteLine($"Account với ID {id} không tìm thấy.");
				return NotFound();
			}

            var dentist = await _context.Dentists.FirstOrDefaultAsync(d => d.AccountID == id);
            if (dentist == null)
            {
                Console.WriteLine($"Dentist với AccountID {id} không tìm thấy.");
                return NotFound();
            }

            var accountVM = new EditAccountVM
			{
				Id = account.ID,
				FirstName = account.FirstName,
				LastName = account.LastName,
				Username = account.Username,
				Email = account.Email,
				DateOfBirth = account.DateOfBirth,
				PhoneNumber = account.PhoneNumber,
				Gender = account.Gender,
				Province = account.Province,
				District = account.District,
				Ward = account.Ward,
				Address = account.Address,
				Role = account.Role,
				DegreeID = dentist.DegreeID,
				ClinicID = dentist.ClinicID
			};

            ViewData["DegreeID"] = new SelectList(_context.Degrees, "ID", "Name", dentist.DegreeID);
            ViewData["ClinicID"] = new SelectList(_context.Clinics, "ID", "Name", dentist.ClinicID);
            return View(accountVM);
		}

		[HttpPost]
		//[Route("EditAccount")]
		public async Task<IActionResult> EditAccountDentist(EditAccountVM model)
		{
			if (ModelState.IsValid)
			{
				var account = await _context.Accounts.FindAsync(model.Id);
				if (account == null)
				{
					return NotFound();
				}

                if (!string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    if (model.NewPassword != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp.");
                        return View(model);
                    }

                    if (model.NewPassword.Length < 3)
                    {
                        ModelState.AddModelError("NewPassword", "Mật khẩu phải có ít nhất 3 ký tự.");
                        return View(model);
                    }

                    account.Password = DataEncryptionExtensions.ToMd5Hash(model.NewPassword);
                }

                account.ID = model.Id;
                account.FirstName = model.FirstName;
				account.LastName = model.LastName;
				account.Username = model.Username;
				account.Email = model.Email;
				account.DateOfBirth = model.DateOfBirth;
				account.PhoneNumber = model.PhoneNumber;
				account.Gender = model.Gender;
				account.Province = model.Province;
				account.District = model.District;
				account.Ward = model.Ward;
				account.Address = model.Address;
				account.Role = model.Role;

                //Update nha sĩ entity
                var dentist = await _context.Dentists.FirstOrDefaultAsync(d => d.AccountID == model.Id);
                if (dentist != null)
                {
                    dentist.DegreeID = model.DegreeID;
                    dentist.ClinicID = model.ClinicID;
                    _context.Update(dentist);
                }

                _context.Update(account);
				await _context.SaveChangesAsync();

				//Chuyển đến ListAccount
				return RedirectToAction(nameof(ListAccountDentist));
			}

            ViewData["DegreeID"] = new SelectList(_context.Degrees, "ID", "Name", model.DegreeID);
            ViewData["ClinicID"] = new SelectList(_context.Clinics, "ID", "Name", model.ClinicID);
            return View(model);
		}
		#endregion

		#region Tài khoản bị khóa (Delete)
		//===================KHÓA TÀI KHOẢN===================
		//[Route("HiddenAccountStatus")]
		[HttpPost]
		public async Task<IActionResult> HiddenAccountStatus(string username, string status)
		{
			var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Username == username);

			if (account != null)
			{
				account.AccountStatus = status;
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(ListAccountDentist));
		}
		#endregion
	}
}
