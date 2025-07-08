using Dental_Clinic_System.Services.VNPAY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Dental_Clinic_System.ViewModels;
using Dental_Clinic_System.Services.MOMO;
using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Models.Data;
using Azure;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using Dental_Clinic_System.Services.EmailSender;
using Microsoft.EntityFrameworkCore;
using static Dental_Clinic_System.Services.VNPAY.VNPAYLibrary;
using Dental_Clinic_System.Services;
namespace Dental_Clinic_System.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVNPayment _vnPayment;
        private readonly IMOMOPayment _momoPayment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly DentalClinicDbContext _context;
        private readonly IEmailSenderCustom _emailSender;

        public PaymentController(IVNPayment vnPayment, IMOMOPayment momoPayment, IHttpClientFactory httpClientFactory, IConfiguration config, DentalClinicDbContext context, IEmailSenderCustom emailSender)
        {
            _vnPayment = vnPayment;
            _httpClientFactory = httpClientFactory;
            _configuration = config;
            _context = context;
            _momoPayment = momoPayment;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "Bệnh Nhân")]
        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(int scheduleID, int patientRecordID, int specialtyID, decimal totalDeposit, string paymentMethod, int clinicID)
        {
            var patient = _context.PatientRecords.FirstOrDefault(p => p.ID == patientRecordID);

            switch (paymentMethod)
            {
                case "VNPAY":
                    var vnpayModel = new VNPaymentRequestModel
                    {
                        Amount = totalDeposit,
                        CreatedDate = DateTime.Now,
                        FullName = patient.FullName,
                        Description = "Thanh toán tiền đặt cọc",

                        // For Appointment Info
                        ScheduleID = scheduleID,
                        PatientRecordID = patientRecordID,
                        SpecialtyID = specialtyID

                    };
                    TempData.SetObjectAsJson("VNPaymentRequestModel", vnpayModel);
                    return Redirect(_vnPayment.CreatePaymentURL(HttpContext, vnpayModel));
                case "MOMO":
                    var momoModel = new MOMOPaymentRequestModel
                    {
                        OrderID = Guid.NewGuid().ToString(),
                        OrderInformation = "Thanh toán tiền đặt cọc",
                        FullName = patient.FullName,
                        Amount = (long)totalDeposit,

                        // For Appointment Info
                        ScheduleID = scheduleID,
                        PatientRecordID = patientRecordID,
                        SpecialtyID = specialtyID
                    };
                    TempData.SetObjectAsJson("MOMOPaymentRequestModel", momoModel);
                    TempData["ScheduleIDTempData"] = scheduleID;
                    TempData["SpecialtyIDTempData"] = specialtyID;
                    TempData["PatientRecordIDTempData"] = patientRecordID;
                    TempData["ClinicIDTempData"] = clinicID;

                    var paymentResult = _momoPayment.CreatePaymentURL(momoModel).Result;
                    if(!paymentResult.payUrl.IsNullOrEmpty())
                    {
                        return Redirect(paymentResult.payUrl);
                    }
                    ViewBag.ResultCode = paymentResult.errorCode;
                    ViewBag.Message = paymentResult.localMessage;
                    return View("PaymentResult");
            }
            return View();
        }

        [Authorize(Roles = "Bệnh Nhân")]
        public IActionResult Index()
        {
            var momoModel = new MOMOPaymentRequestModel
            {
                OrderID = Guid.NewGuid().ToString(),
                OrderInformation = "Thanh toán tiền đặt cọc",
                Amount = 10000,

                // For Appointment Info
                ScheduleID = 0,
                PatientRecordID = 0,
                SpecialtyID = 0,

                // For Disbursement Info
                RequestType = "disburseToBank",
                BankAccountNo = "000000000",
                BankAccountHolderName = "NGUYEN VAN A",
                BankCode = "VCB"
			};
            var vnpayModel = new VNPaymentRequestModel
            {
                Amount = 50000,
                CreatedDate = DateTime.Now,
                FullName = "aaaa",
                Description = "Thanh toán tiền đặt cọc",

                // For Appointment Info
                ScheduleID = 0,
                PatientRecordID = 0,
                SpecialtyID = 0

            };
            return Redirect(_momoPayment.CreatePaymentURL(momoModel).Result.payUrl ?? "paymentresult");
            //return Redirect(_vnPayment.CreatePaymentURL(HttpContext, vnpayModel));
        }

        [Authorize(Roles = "Bệnh Nhân")]
        public IActionResult PaymentResult()
        {
            return View();
        }

        [Authorize(Roles = "Bệnh Nhân")]
        public async Task<IActionResult> PaymentInvoice(int appointmentID, Transaction transaction, int clinicID)
        {
			var specialtySchedulePatientRecord = await _context.Appointments.Include(s => s.Specialty).Include(sc => sc.Schedule).ThenInclude(t => t.TimeSlot).Include(p => p.PatientRecords).ThenInclude(a => a.Account).FirstOrDefaultAsync(a => a.ID == appointmentID);

            Console.WriteLine($"AppoinmentID = {appointmentID}");
			Console.WriteLine($"aaaa = {specialtySchedulePatientRecord?.Specialty?.Name ?? "nullllll"}");

            ViewBag.specialtySchedulePatientRecord = specialtySchedulePatientRecord;
            ViewBag.transaction = transaction;
			ViewBag.clinic = _context.Clinics.FirstOrDefault(c => c.ID == clinicID);

			return View();
        }

        [Authorize(Roles = "Bệnh Nhân")]
        public IActionResult RefundSuccess()
        {
            Console.WriteLine("Refund Successfully!!!");
            return View();
        }

        [Authorize(Roles = "Bệnh Nhân")]
        public IActionResult RefundFail()
        {
            return View();
        }


        // For VN PaymentReturnURL

        [Authorize(Roles = "Bệnh Nhân")]
        public IActionResult PaymentCallBack() // Do bên hệ thống tự động gọi PaymentCallBack
        {
            var response = _vnPayment.PaymentExecute(Request.Query);

            Console.WriteLine($"Response Code = {response.VnPayResponseCode}");

            if (response == null || response.VnPayResponseCode != "00")
            {
                ViewBag.VnPayResponseCode = response.VnPayResponseCode;
                ViewBag.Message = "Lỗi thanh toán VN Pay";
                return View("PaymentResult");
            }

            // Đang test nên tạm thời chưa lưu vào database

            // Lưu đơn hàng vô database
            // Truy xuất đối tượng từ TempData
            //var vnpayModel = TempData.GetObjectFromJson<VNPaymentRequestModel>("VNPaymentRequestModel");
            //var appointment = new Appointment
            //{
            //    ScheduleID = vnpayModel.ScheduleID,
            //    PatientRecordID = vnpayModel.PatientRecordID,
            //    SpecialtyID = vnpayModel.SpecialtyID,
            //    TotalPrice = vnpayModel.Amount,
            //    CreatedDate = DateTime.Now,
            //    AppointmentStatus = "Chờ Xác Nhận"
            //};

            //_context.Appointments.Add(appointment);
            //_context.SaveChanges();

            //var transaction = new Transaction
            //{
            //    AppointmentID = appointment.ID,
            //    Date = DateTime.Now,
            //    BankName = response.BankCode,
            //    TransactionCode = response.TransactionId,
            //    PaymentMethod = response.PaymentMethod,
            //    TotalPrice = response.Amount,
            //    BankAccountNumber = "9704198526191432198",
            //    FullName = vnpayModel.FullName,
            //    Message = response.OrderDescription,
            //    Status = "Thành Công"
            //};

            //_context.Transactions.Add(transaction);
            //_context.SaveChanges();


            string responseCode = response.VnPayResponseCode;
            ViewBag.VnPayResponseCode = responseCode;
            ViewBag.Message = "Thanh toán VNPay thành công";
            return View("PaymentResult");
        }


        // For MOMO PaymentReturnURL (API)

        [HttpGet]
        public async Task<IActionResult> ReturnUrl(string partnerCode, string orderId, string requestId, string amount, string orderInfo, string orderType, string transId, int resultCode, string message, string payType, long responseTime, string extraData, string signature)
        {
            // Kiểm tra chữ ký (signature)
            string rawHash = $"partnerCode={_configuration["MomoAPI:PartnerCode"]}&accessKey={_configuration["MomoAPI:AccessKey"]}&requestId={requestId}&amount={amount.ToString()}&orderId={orderId}&orderInfo={orderInfo}&returnUrl={_configuration["MomoAPI:ReturnUrl"]}&notifyUrl={_configuration["MomoAPI:NotifyUrl"]}&extraData=";
            string secretKey = _configuration["MomoAPI:SecretKey"];
            
            string signatureCheck = DataEncryptionExtensions.SignSHA256(rawHash, secretKey);
            string signatureFromRequest = signatureCheck;

            //Console.WriteLine($"Signature Check : {signatureCheck}");
            //Console.WriteLine($"Signature from Request: {signatureFromRequest}");

            // Debug HERE if you get trouble LOL
            //Console.WriteLine("----------------------------------------------------------------");
            //await Console.Out.WriteLineAsync($"{orderType} || {orderInfo} || {transId} || {resultCode} || {message} || {payType} || {responseTime} || {extraData} || {signature}");

            if (signatureCheck != signatureFromRequest)
            {
                Console.WriteLine("Thành công 1 nửa");
                // Chữ ký không hợp lệ
                return View("PaymentFail");
            }

            if (resultCode == 0 && message == "Success")
            {
                // Thanh toán thành công
                // Lưu vào database

                int scheduleID = (int)TempData["ScheduleIDTempData"];
                int patientRecordID = (int)TempData["PatientRecordIDTempData"];
                int specialtyID = (int)TempData["SpecialtyIDTempData"];
                int clinicID = (int)TempData["ClinicIDTempData"];

                if(_context.Schedules.FirstOrDefault(s => s.ID == scheduleID).ScheduleStatus == "Đã Đặt")
                {
                    await _momoPayment.RefundPayment(long.Parse(amount), long.Parse(transId), message);
                    ViewBag.ResultCode = 999;
                    ViewBag.Message = "Slot này đã có người đặt".ToUpper();
                    return View("PaymentResult");
                }

                var response = new MOMOPaymentResponseModel
                {
                    OrderType = orderType,
                    OrderInfo = orderInfo,
                    TransId = transId,
                    ResultCode = requestId,
                    Message = message,
                    PayType = payType,
                    Amount = Decimal.Parse(amount),
                    ResponseTime = responseTime,
                    ExtraData = extraData,
                    Signature = signature
                };

                // Truy xuất đối tượng từ TempData
                var momoModel = TempData.GetObjectFromJson<MOMOPaymentRequestModel>("MOMOPaymentRequestModel");
                //Console.WriteLine("==================================");
                //await Console.Out.WriteLineAsync($"ScheduleID = {momoModel.ScheduleID}");
                //Console.WriteLine($"ScheduleID from Tempdata = {(int)TempData["SchduleIDTempData"]}");
                //Console.WriteLine($"Amount = {momoModel.Amount}");
                //Console.WriteLine("==================================");

                var appointment = new Appointment
                {
                    ScheduleID = scheduleID,
                    PatientRecordID = patientRecordID,
                    SpecialtyID = specialtyID,
                    TotalPrice = momoModel.Amount,
                    CreatedDate = DateTime.Now,
                    AppointmentStatus = "Chờ Xác Nhận"
                };

                _context.Appointments.Add(appointment);
                _context.SaveChanges();

                var transaction = new Transaction
                {
                    AppointmentID = appointment.ID,
                    Date = DateTime.Now,
                    BankName = response.OrderType,
                    TransactionCode = response.TransId,
                    PaymentMethod = "MOMO",
                    TotalPrice = response.Amount,
                    BankAccountNumber = "9704198526191432198",
                    FullName = momoModel.FullName,
                    Message = response.OrderInfo,
                    Status = "Thành Công"
                };

                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                _context.Schedules.FirstOrDefault(s => s.ID == scheduleID).ScheduleStatus = "Đã Đặt";
                _context.SaveChanges();

				var specialtySchedulePatientRecord = await _context.Appointments.Include(s => s.Specialty).Include(sc => sc.Schedule).ThenInclude(t => t.TimeSlot).Include(p => p.PatientRecords).ThenInclude(a => a.Account).FirstOrDefaultAsync(a => a.ID == appointment.ID);

                ViewBag.appoinmentID = appointment.ID;
                ViewBag.transaction = transaction;
                ViewBag.clinicID = clinicID;

				await _emailSender.SendInvoiceEmailAsync(appointment, transaction, clinicID, "Xác Nhận Phiếu Khám");

                ViewBag.ResultCode = resultCode;
                ViewBag.Message = message.ToUpper();
                return View("PaymentResult");
            }
            else
            {
                // Thanh toán thất bại
                await Console.Out.WriteLineAsync("Fail from Payment Controller");
                ViewBag.ResultCode = resultCode;
                ViewBag.Message = message.ToUpper();
                return View("PaymentResult");
            }
        }


        #region RefundPayment MOMO API
        //[HttpPost]
        //public async Task<IActionResult> RefundPayment(long amount = 10000, long transId = 4063782576, string description = "")
        //{
        //    string endpoint = _configuration["MomoAPI:MomoApiRefundUrl"];
        //    string partnerCode = _configuration["MomoAPI:PartnerCode"];
        //    string accessKey = _configuration["MomoAPI:AccessKey"];
        //    string secretKey = _configuration["MomoAPI:SecretKey"];
        //    string orderId = Guid.NewGuid().ToString();
        //    string requestId = Guid.NewGuid().ToString();
        //    string lang = "vi"; // or "vi"

        //    // Tạo chữ ký (signature)
        //    string rawHash = $"accessKey={accessKey}&amount={amount}&description=&orderId={orderId}&partnerCode={partnerCode}&requestId={requestId}&transId={transId}";

        //    string signature = DataEncryptionExtensions.SignSHA256(rawHash, secretKey);

        //    var refundRequest = new
        //    {
        //        partnerCode,
        //        orderId,
        //        requestId,
        //        amount,
        //        transId,
        //        lang,
        //        description,
        //        signature
        //    };

        //    string jsonRefundRequest = JsonConvert.SerializeObject(refundRequest);
        //    Console.WriteLine("JSON Request: " + jsonRefundRequest);
        //    Console.WriteLine("Raw Hash: " + rawHash);
        //    Console.WriteLine("Signature: " + signature);

        //    using (var client = new HttpClient())
        //    {
        //        var content = new StringContent(jsonRefundRequest, Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync(endpoint, content);
        //        var responseString = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine("JSON Response: " + responseString);

        //        var responseObject = JsonConvert.DeserializeObject<MOMORefundResponseModel>(responseString);

        //        if (responseObject != null && responseObject.resultCode == 0)
        //        {
        //            // Hoàn tiền thành công
        //            //return View("RefundSuccess", responseObject);
        //            return RedirectToAction("RefundSuccess", "payment");

        //        }
        //        else
        //        {
        //            // Xử lý lỗi
        //            //return View("RefundFail", responseObject);
        //            return RedirectToAction("RefundFail", "payment");
        //        }
        //    }
        //}
        #endregion



        #region Disbursement MOMO API

        //      [HttpPost]
        //public async Task<IActionResult> DisburseSingle(MOMOPaymentRequestModel model)
        //{
        //	string endpoint = _configuration["MomoAPI:MomoApiDisbursementUrl"]; //
        //	string partnerCode = _configuration["MomoAPI:PartnerCode"];         //
        //	string accessKey = _configuration["MomoAPI:AccessKey"];
        //	string secretKey = _configuration["MomoAPI:SecretKey"];
        //	string orderInfo = model.OrderInformation;
        //	string returnUrl = _configuration["MomoAPI:ReturnUrl"];
        //	string notifyUrl = _configuration["MomoAPI:NotifyUrl"];
        //	string requestType = "disburseToBank";                             //
        //	string amount = model.Amount.ToString();                            // Số tiền thanh toán
        //	string orderId = model.OrderID;                                     //
        //	string requestId = orderId;
        //	string extraData = "";
        //	string lang = "vi"; // or "vi"

        //	var disbursementMethod = new Dictionary<string, string>()
        //	{
        //              // Disburse To Bank
        //              { "BankAccountNo", "000000000" ?? ""},
        //              //{ "BankCardNo", model.BankCardNo ?? "" },
        //              { "BankAccountHolderName", "NGUYEN VAN A" ?? "" },
        //		{ "BankCode", "VCB" },

        //              // Disburse To Wallet
        //              //{ "WalletId", model.WalletId ?? ""},
        //              //{ "WalletName", model.WalletName ?? "" }
        //          };

        //	// Serialize dictionary to JSON
        //	string disbursementMethodJson = JsonConvert.SerializeObject(disbursementMethod);

        //	var rsa = DataEncryptionExtensions.EncryptRSA(disbursementMethodJson, _configuration["MomoAPI:PublicKey"]);

        //	// Tạo chữ ký (signature)
        //	string rawHash = $"accessKey={accessKey}&amount={amount.ToString()}&disbursementMethod={rsa}&extraData=&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&requestId={requestId}&requestType={requestType}";
        //	string signature = DataEncryptionExtensions.SignSHA256(rawHash, secretKey);

        //	var disbursementRequest = new
        //	{
        //		partnerCode,
        //		orderId,
        //		amount,
        //		requestId,
        //		requestType,
        //		model.DisbursementMethodRSA,
        //		extraData,
        //		orderInfo,
        //		lang,
        //		signature
        //	};

        //	string jsonPaymentRequest = JsonConvert.SerializeObject(disbursementRequest);
        //	Console.WriteLine("JSON Request: " + jsonPaymentRequest);

        //	using (var client = new HttpClient())
        //	{
        //		client.Timeout = TimeSpan.FromSeconds(30);
        //		var content = new StringContent(jsonPaymentRequest, Encoding.UTF8, "application/json");
        //		var response = await client.PostAsync(endpoint, content);
        //		var responseString = await response.Content.ReadAsStringAsync();
        //		Console.WriteLine("JSON Response: " + responseString);

        //		var responseObject = JsonConvert.DeserializeObject<MOMOPaymentResponseModel>(responseString);

        //		if (responseObject != null && !string.IsNullOrEmpty(responseObject.payUrl))
        //		{
        //			return View("RefundSuccess");
        //		}
        //		else
        //		{
        //			Console.WriteLine("Fail!!!");
        //			// Xử lý lỗi
        //			return View("RefundFail");
        //		}
        //	}

        //}

        #endregion

        // ----------------------------------------------------------------------- //
        // Re-Fun VNPAY START

        [HttpPost]
        [Authorize(Roles = "Bệnh Nhân")]
        public async Task<IActionResult> ProcessRefund(string txnRef = "638543936617919359", int amount = 50000)
        {
            var requestId = DateTime.Now.Ticks.ToString();
            var version = _configuration["VNPAY:Version"];
            var command = _configuration["VNPAY:RefundCommand"];
            var tmnCode = _configuration["VNPAY:TmnCode"];
            var transactionType = "02"; // hoặc "03" cho hoàn trả một phần
            var orderInfo = "Refund for order";
            var transactionDate = "20240619113528";
            var transactionNo = "";
            var amounta = (amount * 100);
			var createBy = "admin";
            var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var ipAddr = Utils.GetIpAddress(HttpContext);
            var secureHash = _configuration["VNPAY:HashSecret"];

            // Tạo chuỗi dữ liệu để tính checksum
            string data = $"{requestId}|{version}|{command}|{tmnCode}|{transactionType}|{txnRef}|{amount}|{transactionNo}|{transactionDate}|{createBy}|{createDate}|{ipAddr}|{orderInfo}";

            // Tính checksum
            string secureHashed = ComputeSHA256Hash(secureHash, data);

            var jsonData = new
            {
                vnp_RequestId = requestId,
                vnp_Version = version,
               vnp_Command = command,
                vnp_TmnCode = tmnCode,
                vnp_TransactionType = transactionType,
                vnp_TxnRef = txnRef,
                vnp_Amount = amounta,
                vnp_OrderInfo = orderInfo,
                vnp_TransactionNo = transactionNo,
                vnp_TransactionDate = transactionDate,
                vnp_CreateBy = createBy,
                vnp_CreateDate = createDate,
                vnp_IpAddr = ipAddr,
                vnp_SecureHash = secureHashed
            };

            var jsonString = JsonConvert.SerializeObject(jsonData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            Console.WriteLine("JSON STRING: " + jsonString);

            using (var client = _httpClientFactory.CreateClient())
            {
                //var response = await client.PostAsync(_configuration["VNPAY:RefundAPI"], content);
                var response = await client.PostAsync(_configuration["VNPAY:RefundURL"], content);
                var responseString = await response.Content.ReadAsStringAsync();
				Console.WriteLine("JSON Response: " + responseString);
                return RedirectToAction("index", "home");
				//return ProcessResponse(responseString);
            }
        }



        [Authorize(Roles = "Bệnh Nhân")]
        private IActionResult ProcessResponse(string responseString)
        {
			//var jsonResponse = JObject.Parse(responseString);
			var jsonResponse = JsonConvert.DeserializeObject<VNPaymentRefundRequestModel>(responseString);
			//Console.WriteLine("JSON Response: " + responseString);
			var responseCode = jsonResponse.OrderInfo;
			Console.WriteLine($"Response Code = {responseCode}");
            if (responseCode == "00")
            {
                return RedirectToAction("RefundSuccess");
            }
            else
            {
                return RedirectToAction("RefundFail");
            }
        }

        public static string ComputeSHA256Hash(string secretKey, string rawData)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        //[Authorize(Roles = "Bệnh Nhân")]
        //public IActionResult RefundCallBack()
        //{
        //    var response = _vnPayment.RefundExecute(Request.Query);
        //    Console.WriteLine($"Response Code = {response.VnPayResponseCode}");

        //    if (response == null || response.VnPayResponseCode != "00")
        //    {
        //        TempData["Message"] = $"Lỗi hoàn tiền VN Pay: {response.VnPayResponseCode}";
        //        return RedirectToAction("RefundFail");
        //    }

        //    TempData["Message"] = $"Hoàn tiền VNPay thành công";
        //    return RedirectToAction("RefundSuccess");
        //}

        // Re-Fun VNPAY END
    }
}
