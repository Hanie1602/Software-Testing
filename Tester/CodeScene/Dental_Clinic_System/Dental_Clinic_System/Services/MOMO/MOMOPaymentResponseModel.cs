namespace Dental_Clinic_System.Services.MOMO
{
	public class MOMOPaymentResponseModel
	{
        //public string partnerCode { get; set; } = string.Empty;
        //public string orderId { get; set; } = string.Empty;
        //public string requestId { get; set; } = string.Empty;
        //public long amount { get; set; }
        public string OrderInfo {  get; set; }
        public string OrderType { get; set; }
        public string TransId { get; set; }
        public string ResultCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string PayType { get; set; }
        public decimal Amount { get; internal set; }
        public long ResponseTime { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }

        // For MOMO Response API
        public string payUrl { get; set; } = string.Empty;
        public int errorCode { get; set; }
        public string message { get; set; } = string.Empty;
        public string localMessage { get; set; } = string.Empty;

        

        // For Single Disbursement
        public long? Balance { get; internal set; }
        //public string deeplink { get; set; } = string.Empty;
        //public string qrCodeUrl { get; set; } = string.Empty;
    }
}
