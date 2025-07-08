namespace Dental_Clinic_System.Areas.Admin.ViewModels
{
	public class TransactionVM
	{
		public int Id { get; set; }

		public string? TransactionCode { get; set; }

		public DateTime Date { get; set; }

		public string BankAccountNumber { get; set; }

		public string BankName { get; set; }

		public string PaymentMethod {  get; set; }

		public decimal? TotalPrice { get; set; }

		public string Status { get; set; }

		public string? Message { get; set; }
	}
}
