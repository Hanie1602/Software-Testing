namespace Dental_Clinic_System.Services.MOMO
{
	public class MOMORefundResponseModel
	{
		public string partnerCode { get; set; } = string.Empty;
		public string orderId { get; set; } = string.Empty;
		public string requestId { get; set; } = string.Empty;
		public string extraData { get; set; } = string.Empty;
		public long amount { get; set; }
		public long transId { get; set; }
		public int resultCode { get; set; }
		public string message { get; set; } = string.Empty;
		public long responseTime { get; set; }
	}
}
