namespace Dental_Clinic_System.Services.VNPAY
{
	public class VNPaymentRefundRequestModel
	{
		public string RequestId { get; set; } = DateTime.Now.Ticks.ToString(); // Unique request ID
		public string TransactionId { get; set; } = string.Empty;
		public string TnxRef { get; set; }
		public long Amount { get; set; } // Amount to be refunded
		public string OrderInfo { get; set; }
		public DateTime TransactionDate { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreateBy { get; set; } // User who initiates the refund
	}
}
