//==============================================TÀI KHOẢN BỊ KHÓA================================================

using AutoMapper;
using Dental_Clinic_System.Areas.Admin.Models;
using Dental_Clinic_System.Areas.Admin.ViewModels;
using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;

namespace Dental_Clinic_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "GetAppointmentStatus", Roles = "Admin")]
    //[Route("Admin/[controller]")]
    public class AccountLockedController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public AccountLockedController(DentalClinicDbContext context)
        {
            _context = context;
        }

        #region Show List Account bị khóa
        //[Route("ListLockedAccount")]
        public async Task<IActionResult> ListLockedAccount()
        {
            var lockedAccount = await _context.Accounts
                .Where(a => a.AccountStatus == "Bị Khóa")
                .ToListAsync();

            var lockedAccountList = lockedAccount.Select(a => new ManagerAccountVM
            {
                Id = a.ID,
                Username = a.Username,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                Address = a.Address,
                Role = a.Role
            }).ToList();

            return View(lockedAccountList);
        }
        #endregion

        #region Tìm kiếm trong List Account bị khóa
        //===================TÌM KIẾM TÀI KHOẢN BỊ KHÓA===================
        //[Route("SearchLockedAccount")]
        public async Task<IActionResult> SearchLockedAccount(string keyword)
        {
            //Nếu keyword rỗng, chuyển hướng đến ListLockedAccount
            if (string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction(nameof(ListLockedAccount));
            }

            var accountLocked = await _context.Accounts
                .Where(a => a.Username.Contains(keyword) && a.AccountStatus == "Bị Khóa")
                .ToListAsync();

            var accountLockedList = accountLocked.Select(a => new ManagerAccountVM
            {
                Id = a.ID,
                Username = a.Username,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                Address = a.Address,
                Role = a.Role
            }).ToList();

            return View(nameof(ListLockedAccount), accountLockedList);
        }
        #endregion

        #region Bỏ Chặn
        public async Task<IActionResult> UnlockAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account != null && account.AccountStatus == "Bị Khóa")
            {
                account.AccountStatus = "Hoạt Động";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ListLockedAccount));
        }
        #endregion
    }
}
