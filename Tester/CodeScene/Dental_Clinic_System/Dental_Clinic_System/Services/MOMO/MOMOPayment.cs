using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Dental_Clinic_System.Helper;
using Microsoft.AspNetCore.Mvc;
using Dental_Clinic_System.Models.Data;

namespace Dental_Clinic_System.Services.MOMO
{
    public class MOMOPayment : IMOMOPayment
    {
        private readonly IConfiguration _configuration;
        private readonly DentalClinicDbContext _context;

        public MOMOPayment(IConfiguration config, DentalClinicDbContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<MOMOPaymentResponseModel?> CreatePaymentURL(MOMOPaymentRequestModel model)
        {
            string endpoint = _configuration["MomoAPI:MomoApiUrl"];
            string partnerCode = _configuration["MomoAPI:PartnerCode"];
            string accessKey = _configuration["MomoAPI:AccessKey"];
            string secretKey = _configuration["MomoAPI:SecretKey"];
            string orderInfo = model.OrderInformation;
            string returnUrl = _configuration["MomoAPI:ReturnUrl"];
            string notifyUrl = _configuration["MomoAPI:NotifyUrl"];
            string requestType = _configuration["MomoAPI:RequestType"];
            string amount = model.Amount.ToString(); // Số tiền thanh toán
            string orderId = model.OrderID;
            string requestId = orderId;
            string extraData = "";
            string lang = "vi"; // or "vi"

            // Tạo chữ ký (signature)
            string rawHash = $"partnerCode={partnerCode}&accessKey={accessKey}&requestId={requestId}&amount={amount.ToString()}&orderId={orderId}&orderInfo={orderInfo}&returnUrl={returnUrl}&notifyUrl={notifyUrl}&extraData=";

            string signature = DataEncryptionExtensions.SignSHA256(rawHash, secretKey);
            //var signatureJson = JsonConvert.SerializeObject(new SignatureModelForJSON { Signature = signature });
            await Console.Out.WriteLineAsync("----------------------------------");
            await Console.Out.WriteLineAsync($"Signature from MOMOPAYMENT = {signature}");

            var paymentRequest = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount,
                orderId,
                orderInfo,
                returnUrl,
                notifyUrl,
                extraData,
                lang,
                requestType,
                signature
            };

            string jsonPaymentRequest = JsonConvert.SerializeObject(paymentRequest);
            Console.WriteLine("JSON Request: " + jsonPaymentRequest);

            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonPaymentRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Response: " + responseString);

                var responseObject = JsonConvert.DeserializeObject<MOMOPaymentResponseModel>(responseString);
                if (responseObject != null && !string.IsNullOrEmpty(responseObject.payUrl))
                {
                    //var paymentURL = responseObject.payUrl;
                    return responseObject;
                }
                else
                {
                    Console.WriteLine("Fail from MOMO Payment");
                    // Xử lý lỗi
                    return responseObject;
                }
            }
        }

        public async Task<MOMOPaymentResponseModel?> DisburseSingle(MOMOPaymentRequestModel model)
        {
            string endpoint = _configuration["MomoAPI:MomoApiDisbursementUrl"]; //
            string partnerCode = _configuration["MomoAPI:PartnerCode"];         //
            string accessKey = _configuration["MomoAPI:AccessKey"];
            string secretKey = _configuration["MomoAPI:SecretKey"];
            string orderInfo = model.OrderInformation;
            string returnUrl = _configuration["MomoAPI:ReturnUrl"];
            string notifyUrl = _configuration["MomoAPI:NotifyUrl"];
            string requestType = model.RequestType;                             //
            string amount = model.Amount.ToString();                            // Số tiền thanh toán
            string orderId = model.OrderID;                                     //
            string requestId = orderId;
            string extraData = "";
            string lang = "vi"; // or "vi"
            var disbursementMethod = new Dictionary<string, string>()
            {
                // Disburse To Bank
                { "BankAccountNo", model.BankAccountNo ?? ""},
                //{ "BankCardNo", model.BankCardNo ?? "" },
                { "BankAccountHolderName", model.BankAccountHolderName ?? "" },
                { "BankCode", model.BankCode },

                // Disburse To Wallet
                //{ "WalletId", model.WalletId ?? ""},
                //{ "WalletName", model.WalletName ?? "" }
            };

            //model.DisbursementMethodRSA = DataEncryptionExtensions.EncryptRSA(disbursementMethod, _configuration["MomoAPI:PublicKey"]);

            // Tạo chữ ký (signature)
            string rawHash = $"accessKey={accessKey}&amount={amount.ToString()}&disbursementMethod={model.DisbursementMethodRSA}&extraData=&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&requestId={requestId}&requestType={requestType}";
            string signature = DataEncryptionExtensions.SignSHA256(rawHash, secretKey);

            var disbursementRequest = new
            {
                partnerCode,
                orderId,
                amount,
                requestId,
                requestType,
                model.DisbursementMethodRSA,
                extraData,
                orderInfo,
                lang,
                signature
            };

            string jsonPaymentRequest = JsonConvert.SerializeObject(disbursementRequest);
            Console.WriteLine("JSON Request: " + jsonPaymentRequest);

            using (var client = new HttpClient())
            {
				client.Timeout = TimeSpan.FromSeconds(30);
				var content = new StringContent(jsonPaymentRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Response: " + responseString);

                var responseObject = JsonConvert.DeserializeObject<MOMOPaymentResponseModel>(responseString);

                if (responseObject != null && !string.IsNullOrEmpty(responseObject.payUrl))
                {
                    return responseObject;
                }
                else
                {
                    Console.WriteLine("Fail!!!");
                    // Xử lý lỗi
                    return null;
                }
            }

        }

        [HttpPost]
        public async Task<MOMORefundResponseModel?> RefundPayment(long amount, long transId, string description = "")
        {
            string endpoint = _configuration["MomoAPI:MomoApiRefundUrl"];
            string partnerCode = _configuration["MomoAPI:PartnerCode"];
            string accessKey = _configuration["MomoAPI:AccessKey"];
            string secretKey = _configuration["MomoAPI:SecretKey"];
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string lang = "vi"; // or "vi"

            // Tạo chữ ký (signature)
            string rawHash = $"accessKey={accessKey}&amount={amount}&description=&orderId={orderId}&partnerCode={partnerCode}&requestId={requestId}&transId={transId}";

            string signature = DataEncryptionExtensions.SignSHA256(rawHash, secretKey);

            var refundRequest = new
            {
                partnerCode,
                orderId,
                requestId,
                amount,
                transId,
                lang,
                description,
                signature
            };

            string jsonRefundRequest = JsonConvert.SerializeObject(refundRequest);
            Console.WriteLine("JSON Request: " + jsonRefundRequest);
            Console.WriteLine("Raw Hash: " + rawHash);
            Console.WriteLine("Signature: " + signature);

            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonRefundRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Response: " + responseString);

                var responseObject = JsonConvert.DeserializeObject<MOMORefundResponseModel>(responseString);

                if (responseObject != null && responseObject.resultCode == 0)
                {
                    // Hoàn tiền thành công
                    //return View("RefundSuccess", responseObject);
                    return responseObject;

                }
                else
                {
                    // Xử lý lỗi
                    //return View("RefundFail", responseObject);
                    return null;
                }
            }
        }
    }
}
