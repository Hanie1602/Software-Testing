using Dental_Clinic_System.Areas.Admin.ViewModels;
using Dental_Clinic_System.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Dental_Clinic_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "GetAppointmentStatus", Roles = "Admin")]
    public class TransactionController : Controller
    {
        private readonly DentalClinicDbContext _context;

        public TransactionController(DentalClinicDbContext context)
        {
            _context = context;
        }

        //[Route("TransactionHistory")]
        public async Task<IActionResult> TransactionHistory()
        {
            var transactionHistory = await _context.Transactions.ToListAsync();

            var transactionList = transactionHistory.Select(t => new TransactionVM
            {
                Id = t.ID,
                TransactionCode = t.TransactionCode,
                Date = t.Date,
                BankName = t.BankName,
                BankAccountNumber = t.BankAccountNumber,
                TotalPrice = t.TotalPrice,
                Message = t.Message,
                Status = t.Status,
			}).ToList();

            return View(transactionList);
        }
    }
}
