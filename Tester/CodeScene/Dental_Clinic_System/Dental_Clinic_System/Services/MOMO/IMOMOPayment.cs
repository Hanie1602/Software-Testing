namespace Dental_Clinic_System.Services.MOMO
{
    public interface IMOMOPayment
    {
        Task<MOMOPaymentResponseModel?> CreatePaymentURL(MOMOPaymentRequestModel model);
        Task<MOMORefundResponseModel?> RefundPayment(long amount, long transId, string description);
        Task<MOMOPaymentResponseModel?> DisburseSingle(MOMOPaymentRequestModel model);
    }
}
