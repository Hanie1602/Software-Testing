namespace Dental_Clinic_System.Services.VNPAY
{
    public interface IVNPayment
    {
        string CreatePaymentURL(HttpContext context, VNPaymentRequestModel model);
        string CreateRefundURL(HttpContext context, VNPaymentRefundRequestModel model);
        VNPaymentResponseModel PaymentExecute(IQueryCollection collection);
        VNPaymentResponseModel RefundExecute(IQueryCollection collection);
    }
}
