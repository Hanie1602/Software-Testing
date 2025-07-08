using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dental_Clinic_System.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ZXing.QrCode.Internal;
using Dental_Clinic_System.Helper;
using Dental_Clinic_System.Services.EmailVerification;
using System.Globalization;

namespace Dental_Clinic_System.Services.EmailSender
{
    public class EmailSender : IEmailSenderCustom
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;
        private readonly DentalClinicDbContext _context;
        private readonly IEmailVerification _emailVerification;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger, DentalClinicDbContext context, IEmailVerification emailVerification)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _emailVerification = emailVerification;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Get username from the email
            var user = _context.Accounts.FirstOrDefault(u => u.Email == email);
            string username = user?.Username;

            try
            {
                // Verify email before sending
                //bool isValidEmail = await _emailVerification.VerifyEmailAsync(email);

                //if (!isValidEmail)
                //{
                //    _logger.LogWarning($"Email {email} is not valid. Aborting email send.");
                //    return;
                //}

                var smtpClient = new SmtpClient(_configuration["Email:Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Email:Smtp:Port"]),
                    Credentials = new NetworkCredential(_configuration["Email:Smtp:Username"], _configuration["Email:Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:FromAddress"], _configuration["Email:FromName"]),
                    Subject = subject,
                    Body = $"<tbody>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit\" height=\"24\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><h1 style=\"font-size:32px;font-weight:500;letter-spacing:0.01em;color:#141212;text-align:center;line-height:39px;margin:0;font-family:inherit\">Xác Minh Email Của Bạn</h1></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td></tr></tbody></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                <table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\">\r\n                    <tbody>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit\" height=\"64\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td>\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                <table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;background-color:#f9f9f9;border-collapse:collapse\" width=\"100%\" bgcolor=\"#F9F9F9\">\r\n                                    <tbody>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit\" height=\"40\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit\" width=\"38\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:38px;font-family:inherit\"></div></td>\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                                <table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse\" width=\"100%\">\r\n                                                    <tbody>\r\n                                                        <tr><td><h2 style=\"font-size:25.63px;font-weight:700;line-height:100%;color:#333;margin:0;text-align:center;font-family:inherit\">{username} </h2></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:8px;border-collapse:collapse;font-family:inherit\" height=\"8\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><p style=\"margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit\">Bạn đã xác nhận địa chỉ email của Tài khoản Dental Care. Vui lòng xác minh email để xác nhận.<br>Nếu bạn không yêu cầu bất kỳ thay đổi nào, hãy xóa email này. Nếu có thắc mắc, vui lòng liên hệ <a href=\"Rivinger7@gmail.com\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\">Bộ Phận Hỗ Trợ Dental Care</a>.</p></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit\" height=\"40\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                                                <div style=\"font-family:inherit\">\r\n\r\n                                                                    <a href=\"{HtmlEncoder.Default.Encode(message)}\" style=\"min-width:300px;background:#1376f8;border-radius:12.8px;padding:25.5px 19px 26.5px 19px;text-align:center;font-size:18px;font-weight:700;color:#fff;display:inline-block;text-decoration:none;line-height:120%\" target=\"_blank\">Xác Minh Email</a>\r\n\r\n\r\n\r\n\r\n                                                                </div>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </tbody>\r\n                                                </table>\r\n                                            </td>\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit\" width=\"38\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:38px;font-family:inherit\"></div></td>\r\n                                        </tr>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td>\r\n                        </tr>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse\" width=\"100%\" align=\"center\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit\" width=\"100\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:100px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">Nếu bạn không phải là người gửi yêu cầu này, hãy đổi mật khẩu tài khoản ngay lập tức để tránh việc bị truy cập trái phép.</td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit\" width=\"100\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:100px;font-family:inherit\"></div></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:80px;border-collapse:collapse;font-family:inherit\" height=\"80\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:11.24px;line-height:140%;letter-spacing:-0.01em;color:#999;table-layout:fixed;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;display:inline-table;width:auto;border-collapse:collapse\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><a href=\"https://localhost:7165/\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Flogo.png?alt=media&token=c4e58d3c-574a-4050-b4a9-163736bd90cd\" alt=\"Logo Dental Care\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:142px\" width=\"142\" class=\"CToWUd\" ></a></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;height:44px;width:100%;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Facebook\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ffacebookmail.png?alt=media&token=f88279fd-f9ad-455a-a093-5cd854e0e440\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Instagram\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Finstagrammail.png?alt=media&token=8bb0b106-6666-461a-9f94-df6b4d8872fb\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng YouTube\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Fyoutubemail.png?alt=media&token=725229c6-d9a8-4635-95fb-3383df77de83\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Twitter\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ftwittermail.png?alt=media&token=b86058d2-11ce-438f-aa25-915dbfb1c1bb\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:32px;border-collapse:collapse;font-family:inherit\" height=\"32\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"padding:0;border:none;border-spacing:0;width:100%;margin:0 auto;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\" >Chính sách Quyền riêng tư</a></span><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px\" width=\"4\" class=\"CToWUd\" ><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\" >Hỗ trợ</a></span><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px\" width=\"4\" class=\"CToWUd\" ><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\">Điều khoản Sử dụng</a></span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">Đây là dịch vụ thư thông báo.</span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">Tập Đoàn&nbsp;Dental&nbsp;Care, Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 ©&nbsp;2024&nbsp;Dental&nbsp;Care.&nbsp;Đã&nbsp;đăng ký&nbsp;bản quyền</span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">© năm 2024 bởi Tập Đoàn Dental Care, Dental Care, Nền tảng đặt lịch nha sĩ và các logo liên quan là nhãn hiệu, nhãn hiệu dịch vụ và/hoặc nhãn hiệu đã đăng ký của Tập Đoàn Dental Care.</span></td></tr></tbody></table></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit\" height=\"64\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr>\r\n    </tbody>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                // Optional: Add additional headers to improve deliverability
                mailMessage.Headers.Add("X-Priority", "1");
                mailMessage.Headers.Add("X-MSMail-Priority", "High");
                mailMessage.Headers.Add("Importance", "High");

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {email} with subject {subject}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"Error sending email to {email} with subject {subject}");
                throw; // Re-throw the exception if you want the caller to handle it
            }
        }

        public async Task SendEmailForUpdatingAsync(string email, string username, string subject, string message)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Email:Smtp:Port"]),
                    Credentials = new NetworkCredential(_configuration["Email:Smtp:Username"], _configuration["Email:Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:FromAddress"], _configuration["Email:FromName"]),
                    Subject = subject,
                    Body = $"<tbody>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit\" height=\"24\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><h1 style=\"font-size:32px;font-weight:500;letter-spacing:0.01em;color:#141212;text-align:center;line-height:39px;margin:0;font-family:inherit\">Xác Minh Email Của Bạn</h1></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td></tr></tbody></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                <table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\">\r\n                    <tbody>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit\" height=\"64\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td>\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                <table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;background-color:#f9f9f9;border-collapse:collapse\" width=\"100%\" bgcolor=\"#F9F9F9\">\r\n                                    <tbody>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit\" height=\"40\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit\" width=\"38\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:38px;font-family:inherit\"></div></td>\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                                <table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse\" width=\"100%\">\r\n                                                    <tbody>\r\n                                                        <tr><td><h2 style=\"font-size:25.63px;font-weight:700;line-height:100%;color:#333;margin:0;text-align:center;font-family:inherit\">{username} </h2></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:8px;border-collapse:collapse;font-family:inherit\" height=\"8\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><p style=\"margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit\">Bạn đã yêu cầu thay đổi địa chỉ email của Tài khoản Dental Care. Vui lòng xác minh email để xác nhận.<br>Nếu bạn không yêu cầu bất kỳ thay đổi nào, hãy xóa email này. Nếu có thắc mắc, vui lòng liên hệ <a href=\"Rivinger7@gmail.com\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\">Bộ Phận Hỗ Trợ Dental Care</a>.</p></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit\" height=\"40\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                                                <div style=\"font-family:inherit\">\r\n\r\n                                                                    <a href=\"{HtmlEncoder.Default.Encode(message)}\" style=\"min-width:300px;background:#1376f8;border-radius:12.8px;padding:25.5px 19px 26.5px 19px;text-align:center;font-size:18px;font-weight:700;color:#fff;display:inline-block;text-decoration:none;line-height:120%\" target=\"_blank\">Xác Minh Email</a>\r\n\r\n\r\n\r\n\r\n                                                                </div>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </tbody>\r\n                                                </table>\r\n                                            </td>\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit\" width=\"38\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:38px;font-family:inherit\"></div></td>\r\n                                        </tr>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td>\r\n                        </tr>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse\" width=\"100%\" align=\"center\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit\" width=\"100\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:100px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">Nếu bạn không phải là người gửi yêu cầu này, hãy đổi mật khẩu tài khoản ngay lập tức để tránh việc bị truy cập trái phép.</td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit\" width=\"100\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:100px;font-family:inherit\"></div></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:80px;border-collapse:collapse;font-family:inherit\" height=\"80\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:11.24px;line-height:140%;letter-spacing:-0.01em;color:#999;table-layout:fixed;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;display:inline-table;width:auto;border-collapse:collapse\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><a href=\"https://localhost:7165/\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Flogo.png?alt=media&token=c4e58d3c-574a-4050-b4a9-163736bd90cd\" alt=\"Logo Dental Care\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:142px\" width=\"142\" class=\"CToWUd\" ></a></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;height:44px;width:100%;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Facebook\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ffacebookmail.png?alt=media&token=f88279fd-f9ad-455a-a093-5cd854e0e440\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Instagram\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Finstagrammail.png?alt=media&token=8bb0b106-6666-461a-9f94-df6b4d8872fb\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng YouTube\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Fyoutubemail.png?alt=media&token=725229c6-d9a8-4635-95fb-3383df77de83\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Twitter\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ftwittermail.png?alt=media&token=b86058d2-11ce-438f-aa25-915dbfb1c1bb\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:32px;border-collapse:collapse;font-family:inherit\" height=\"32\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"padding:0;border:none;border-spacing:0;width:100%;margin:0 auto;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\" >Chính sách Quyền riêng tư</a></span><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px\" width=\"4\" class=\"CToWUd\" ><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\" >Hỗ trợ</a></span><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px\" width=\"4\" class=\"CToWUd\" ><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\">Điều khoản Sử dụng</a></span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">Đây là dịch vụ thư thông báo.</span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">Tập Đoàn&nbsp;Dental&nbsp;Care, Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 ©&nbsp;2024&nbsp;Dental&nbsp;Care.&nbsp;Đã&nbsp;đăng ký&nbsp;bản quyền</span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">© năm 2024 bởi Tập Đoàn Dental Care, Dental Care, Nền tảng đặt lịch nha sĩ và các logo liên quan là nhãn hiệu, nhãn hiệu dịch vụ và/hoặc nhãn hiệu đã đăng ký của Tập Đoàn Dental Care.</span></td></tr></tbody></table></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit\" height=\"64\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr>\r\n    </tbody>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                // Optional: Add additional headers to improve deliverability
                mailMessage.Headers.Add("X-Priority", "1");
                mailMessage.Headers.Add("X-MSMail-Priority", "High");
                mailMessage.Headers.Add("Importance", "High");

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {email} with subject {subject}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"Error sending email to {email} with subject {subject}");
                throw; // Re-throw the exception if you want the caller to handle it
            }
        }

        public async Task SendEmailUpdatedAsync(string oldEmail, string newEmail, string subject, string message)
        {
            // Get username from the email
            var user = _context.Accounts.FirstOrDefault(u => u.Email == oldEmail);
            string username = user?.Username;
            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Email:Smtp:Port"]),
                    Credentials = new NetworkCredential(_configuration["Email:Smtp:Username"], _configuration["Email:Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:FromAddress"], _configuration["Email:FromName"]),
                    Subject = subject,
                    Body = $"<tbody>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit\" height=\"24\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><h1 style=\"font-size:32px;font-weight:500;letter-spacing:0.01em;color:#141212;text-align:center;line-height:39px;margin:0;font-family:inherit\">Xác Minh Email Của Bạn</h1></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td></tr></tbody></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                <table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\">\r\n                    <tbody>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit\" height=\"64\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td>\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                <table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;background-color:#f9f9f9;border-collapse:collapse\" width=\"100%\" bgcolor=\"#F9F9F9\">\r\n                                    <tbody>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit\" height=\"40\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\">\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit\" width=\"38\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:38px;font-family:inherit\"></div></td>\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">\r\n                                                <table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse\" width=\"100%\">\r\n                                                    <tbody>\r\n                                                        <tr><td><h2 style=\"font-size:25.63px;font-weight:700;line-height:100%;color:#333;margin:0;text-align:center;font-family:inherit\">{username} </h2></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:8px;border-collapse:collapse;font-family:inherit\" height=\"8\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><p style=\"margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit\">Chúng tôi đã thay đổi địa chỉ email cho tài khoản Dental Care của bạn thành công.</p><p style=\"margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit\"><strong>Cũ: </strong> {oldEmail}</p><p style=\"margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit\"><strong>Mới: </strong> {newEmail}</p><p style=\"margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit\">Nếu bạn không yêu cầu thực hiện thay đổi này, vui lòng liên hệ với <a href=\"Rivinger7@gmail.com\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\">Đội Ngũ Hỗ Trợ Dental Care</a>.</p></td></tr>\r\n                                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit\" height=\"40\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                                                                                           </tbody>\r\n                                                </table>\r\n                                            </td>\r\n                                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit\" width=\"38\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:38px;font-family:inherit\"></div></td>\r\n                                        </tr>\r\n                                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                            <td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td>\r\n                        </tr>\r\n                        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse\" width=\"100%\" align=\"center\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit\" width=\"100\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:100px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\">Nếu bạn không phải là người gửi yêu cầu này, hãy đổi mật khẩu tài khoản ngay lập tức để tránh việc bị truy cập trái phép.</td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit\" width=\"100\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:100px;font-family:inherit\"></div></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:80px;border-collapse:collapse;font-family:inherit\" height=\"80\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr>\r\n        <tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:11.24px;line-height:140%;letter-spacing:-0.01em;color:#999;table-layout:fixed;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;display:inline-table;width:auto;border-collapse:collapse\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><a href=\"https://localhost:7165/\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Flogo.png?alt=media&token=c4e58d3c-574a-4050-b4a9-163736bd90cd\" alt=\"Logo Dental Care\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:142px\" width=\"142\" class=\"CToWUd\" ></a></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit\" height=\"48\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;height:44px;width:100%;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Facebook\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ffacebookmail.png?alt=media&token=f88279fd-f9ad-455a-a093-5cd854e0e440\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Instagram\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Finstagrammail.png?alt=media&token=8bb0b106-6666-461a-9f94-df6b4d8872fb\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng YouTube\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Fyoutubemail.png?alt=media&token=725229c6-d9a8-4635-95fb-3383df77de83\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"24\" height=\"44\"></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit\" width=\"44\" height=\"44\"><a href=\"https://www.facebook.com/profile.php?id=100093052218614\" style=\"color:#bd2225;text-decoration:underline\" target=\"_blank\"><img alt=\"Biểu tượng Twitter\" src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ftwittermail.png?alt=media&token=b86058d2-11ce-438f-aa25-915dbfb1c1bb\" style=\"border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px\" width=\"44\" height=\"44\" class=\"CToWUd\" ></a></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:32px;border-collapse:collapse;font-family:inherit\" height=\"32\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" style=\"padding:0;border:none;border-spacing:0;width:100%;margin:0 auto;border-collapse:collapse\" width=\"100%\"><tbody><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\" >Chính sách Quyền riêng tư</a></span><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px\" width=\"4\" class=\"CToWUd\" ><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\" >Hỗ trợ</a></span><span style=\"display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit\"><img src=\"https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670\" style=\"border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px\" width=\"4\" class=\"CToWUd\" ><a href=\"#\" style=\"text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle\" target=\"_blank\">Điều khoản Sử dụng</a></span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">Đây là dịch vụ thư thông báo.</span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">Tập Đoàn&nbsp;Dental&nbsp;Care, Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 ©&nbsp;2024&nbsp;Dental&nbsp;Care.&nbsp;Đã&nbsp;đăng ký&nbsp;bản quyền</span></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"1\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit\" height=\"16\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td style=\"margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit\" align=\"center\"><span style=\"font-family:inherit\">© năm 2024 bởi Tập Đoàn Dental Care, Dental Care, Nền tảng đặt lịch nha sĩ và các logo liên quan là nhãn hiệu, nhãn hiệu dịch vụ và/hoặc nhãn hiệu đã đăng ký của Tập Đoàn Dental Care.</span></td></tr></tbody></table></td><td style=\"margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit\" width=\"72\" height=\"100%\"><div style=\"height:100%;overflow:hidden;width:72px;font-family:inherit\"></div></td></tr><tr style=\"margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit\"><td colspan=\"3\" style=\"margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit\" height=\"64\"><table style=\"margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse\" width=\"100%\"></table></td></tr></tbody></table></td></tr>\r\n    </tbody>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(newEmail);

                // Optional: Add additional headers to improve deliverability
                mailMessage.Headers.Add("X-Priority", "1");
                mailMessage.Headers.Add("X-MSMail-Priority", "High");
                mailMessage.Headers.Add("Importance", "High");

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {newEmail} with subject {subject}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"Error sending email to {newEmail} with subject {subject}");
                throw; // Re-throw the exception if you want the caller to handle it
            }
        }

        public async Task SendInvoiceEmailAsync(Appointment appointment, Transaction transaction, int clinicID, string subject)
        {
            var clinic = _context.Clinics.FirstOrDefaultAsync(c => c.ID == clinicID).Result;

            var specialtySchedulePatientRecord = appointment;

            var toEmail = specialtySchedulePatientRecord?.PatientRecords?.EmailReceiver ?? specialtySchedulePatientRecord?.PatientRecords?.Account?.Email;

            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Email:Smtp:Port"]),
                    Credentials = new NetworkCredential(_configuration["Email:Smtp:Username"], _configuration["Email:Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:FromAddress"], _configuration["Email:FromName"]),
                    Subject = subject,
                    Body = $@"
<html>
<head>
    <meta charset='UTF-8' />
    <meta http-equiv='X-UA-Compatible' content='IE=edge' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Document</title>
    <link rel='preconnect' href='https://fonts.googleapis.com' />
    <link rel='preconnect' href='https://fonts.gstatic.com' crossorigin />
    <link href='https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900&display=swap' rel='stylesheet' />
</head>
<body style='color: #3e3e3e; background: #e8f2f7; font-family: Poppins, sans-serif; font-size: 1.6rem; min-height: 100vh; display: flex; align-items: center; justify-content: center;'>
    <table style='max-width: 600px; margin: auto; background: #fff; border-radius: 20px; box-shadow: 0 0 10px rgba(0,0,0,0.1); border-collapse: collapse;'>
        <!-- HEADER -->
        <tr>
            <td style='padding: 20px 0; text-align: center;'>
                <h1 style='font-size: 1.8rem; font-weight: 700; margin-bottom: 1rem;'>PHIẾU KHÁM BỆNH</h1>
                <p style='font-size: 26px; font-weight: 700; margin-bottom: 5px;'>{clinic?.Name}</p>
                <p style='font-size: 20px; font-weight: 300; color: #3e3e3e;'>{clinic?.Address}</p>
                <h2 style='font-size: 1.4rem; margin-bottom: 10px;'>Mã phiếu</h2>
                <div class='barcode' style='text-align: center;'>
                   <img src='https://barcodeapi.org/api/code128/{transaction?.MedicalReportID}' alt='Barcode' style='max-width: 100%; height: auto;'>
                </div>
            </td>
        </tr>
        <tr>
            <td style='text-align: center;'>
                <div style='font-size: 20px; padding: 10px 28%; border-radius: 20px; background-color: #3bb54a; color: #fff; display: inline-block;'>
                    Đặt khám thành công
                </div>
            </td>
        </tr>
        <!-- TIME -->
        <tr>
            <td style='padding: 20px 0; text-align: center;'>
                <div style='color: #1376f8; font-size: 1.4rem; font-weight: 500;'>Giờ khám dự kiến</div>
                <div style='font-size: 3.2rem; color: #1376f8; font-weight: 700; line-height: normal; margin-bottom: 2rem;'>{appointment?.Schedule?.TimeSlot?.StartTime.ToString("HH:mm")}</div>
                <table style='width: 100%; font-size: 1.3rem; padding: 0 20px;'>
                    <tr>
                        <td style='width: 50%; text-align: left;'>Mã phiếu:</td>
                        <td style='text-align: right;'><b>{transaction?.MedicalReportID}</b></td>
                    </tr>
                    <tr>
                        <td style='text-align: left;'>Chuyên khoa:</td>
                        <td style='text-align: right;'><b>{specialtySchedulePatientRecord?.Specialty.Name}</b></td>
                    </tr>
                    <tr>
                        <td style='text-align: left;'>Ngày khám:</td>
                        <td style='text-align: right; color: #1abc9c;'><b>{specialtySchedulePatientRecord?.Schedule?.Date.ToString("dd/MM/yyyy")}</b></td>
                    </tr>
                    <tr>
                        <td style='text-align: left;'>Giờ khám dự kiến:</td>
                        <td style='text-align: right; color: #1abc9c;'><b>{specialtySchedulePatientRecord?.Schedule?.TimeSlot?.StartTime.ToString("HH:mm")}</b></td>
                    </tr>
                    <tr>
                        <td style='text-align: left;'>Phí khám:</td>
                        <td style='text-align: right;'><b>{string.Format(new CultureInfo("vi-VN"), "{0:#,##0.} đ", appointment?.TotalPrice)}</b></td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- PATIENT INFO -->
        <tr>
            <td style='padding: 20px 0;'>
                <table style='width: 100%; font-size: 1.3rem; padding: 0 20px;'>
                    <tr>
                        <td style='width: 50%; text-align: left;'>Bệnh nhân:</td>
                        <td style='text-align: right;'><b>{specialtySchedulePatientRecord?.PatientRecords?.FullName.ToString()}</b></td>
                    </tr>
                    <tr>
                        <td style='text-align: left;'>Ngày sinh:</td>
                        <td style='text-align: right;'><b>{specialtySchedulePatientRecord?.PatientRecords?.DateOfBirth.ToString("dd/MM/yyyy")}</b></td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- COPYRIGHT -->
        <tr>
            <td style='padding: 20px 0 0; text-align: center;'>
                <div class='note'>
                    <p style='color: #df0000; font-style: italic; font-size: 1.4rem; font-weight: 700; margin-bottom: 1rem;'>Lưu ý:</p>
                    <div style='font-size: 1.4rem; margin-bottom: 10px; font-style: italic; color: #3e3e3e;'>
                        Quý bệnh nhân vui lòng đến quầy tiếp nhận tại sảnh để được tiếp đón.
                        <br />
                        Quý bệnh nhân cần hỗ trợ, vui lòng liên hệ tổng đài <strong>CSKH 1900 2115</strong>
                    </div>
                </div>
                <div style='font-size: 1.3rem;'>
                    <div>Bản quyền thuộc về</div>
                    <div>
                        <img style='width: 50%' src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2FDentalCare.png?alt=media&token=b98bc2f0-f603-4b85-bd1b-764d8679baf0' alt='DentalCare Logo' />
                    </div>
                </div>
                <div style='border-bottom: 2px dashed #f0f2f5; margin-top: 5px; padding-bottom: 30px; font-size: 1rem;'>Đặt lịch khám tại Bệnh viện - Phòng khám hàng đầu Việt Nam</div>
            </td>
        </tr>
    </table>
</body>
</html>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                // Optional: Add additional headers to improve deliverability
                mailMessage.Headers.Add("X-Priority", "1");
                mailMessage.Headers.Add("X-MSMail-Priority", "High");
                mailMessage.Headers.Add("Importance", "High");

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {toEmail} with {subject}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"Email did not send to {toEmail}");
                throw; // Re-throw the exception if you want the caller to handle it
            }
        }

        public async Task SendBusinessPartnershipsInfo(Order order, Account managerAccount, string encryptedPassword, string subject)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Email:Smtp:Port"]),
                    Credentials = new NetworkCredential(_configuration["Email:Smtp:Username"], _configuration["Email:Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:FromAddress"], _configuration["Email:FromName"]),
                    Subject = subject,
                    Body = @$"
<tbody>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit' height='24'>
            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'>
                <tbody>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit' width='72' height='100%'>
                            <div style='height:100%;overflow:hidden;width:72px;font-family:inherit'></div>
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                            <h1 style='font-size:32px;font-weight:500;letter-spacing:0.01em;color:#141212;text-align:center;line-height:39px;margin:0;font-family:inherit'>Xác Nhận Trở Thành Đối Tác Của Dental Care</h1>
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit' width='72' height='100%'>
                            <div style='height:100%;overflow:hidden;width:72px;font-family:inherit'></div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'>
                <tbody>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td colspan='3' style='margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit' height='64'>
                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                        </td>
                    </tr>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit' width='72' height='100%'>
                            <div style='height:100%;overflow:hidden;width:72px;font-family:inherit'></div>
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;background-color:#f9f9f9;border-collapse:collapse' width='100%' bgcolor='#F9F9F9'>
                                <tbody>
                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                        <td colspan='3' style='margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit' height='40'>
                                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                                        </td>
                                    </tr>
                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit' width='38' height='100%'>
                                            <div style='height:100%;overflow:hidden;width:38px;font-family:inherit'></div>
                                        </td>
                                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse' width='100%'>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <h2 style='font-size:25.63px;font-weight:700;line-height:100%;color:#333;margin:0;text-align:center;font-family:inherit'>{"Xin chúc mừng bạn đã tham gia nền tảng Dental Care của chúng tôi"}</h2>
                                                        </td>
                                                    </tr>
                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                                        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:8px;border-collapse:collapse;font-family:inherit' height='8'>
                                                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                                                        </td>
                                                    </tr>
                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                                                            <p style='margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit'>Chúng tôi đã cung cấp cho bạn tài khoản Manager.</p>
                                                            <p style='margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit'><strong>Username: </strong> {managerAccount.Username}</p>
                                                            <p style='margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit'><strong>Password: </strong> {encryptedPassword}</p>
                                                            <p style='margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit'>Vui lòng đổi mật khẩu của bạn khi đăng nhập thành công</p>
                                                            <p style='margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit'>Nếu bạn không yêu cầu thực hiện thay đổi này, vui lòng liên hệ với <a href='mailto:Rivinger7@gmail.com' style='color:#bd2225;text-decoration:underline' target='_blank'>Đội Ngũ Hỗ Trợ Dental Care</a>.</p>
                                                        </td>
                                                    </tr>
                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                                        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit' height='40'>
                                                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit' width='38' height='100%'>
                                            <div style='height:100%;overflow:hidden;width:38px;font-family:inherit'></div>
                                        </td>
                                    </tr>
                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                        <td colspan='3' style='margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit' height='48'>
                                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit' width='72' height='100%'>
                            <div style='height:100%;overflow:hidden;width:72px;font-family:inherit'></div>
                        </td>
                    </tr>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td colspan='3' style='margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit' height='48'>
                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse' width='100%' align='center'>
                <tbody>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit' width='100' height='100%'>
                            <div style='height:100%;overflow:hidden;width:100px;font-family:inherit'></div>
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                            Nếu bạn không phải là người gửi yêu cầu này, hãy đổi mật khẩu tài khoản ngay lập tức để tránh việc bị truy cập trái phép.
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit' width='100' height='100%'>
                            <div style='height:100%;overflow:hidden;width:100px;font-family:inherit'></div>
                        </td>
                    </tr>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td colspan='3' style='margin:0;padding:0;border:none;border-spacing:0;height:80px;border-collapse:collapse;font-family:inherit' height='80'>
                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'>
                <tbody>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit' width='72' height='100%'>
                            <div style='height:100%;overflow:hidden;width:72px;font-family:inherit'></div>
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:11.24px;line-height:140%;letter-spacing:-0.01em;color:#999;table-layout:fixed;border-collapse:collapse' width='100%'>
                                <tbody>
                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                                            <table style='margin:0;padding:0;border:none;border-spacing:0;display:inline-table;width:auto;border-collapse:collapse'>
                                                <tbody>
                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                                                            <a href='https://localhost:7165/' style='color:#bd2225;text-decoration:underline' target='_blank'>
                                                                <img src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Flogo.png?alt=media&token=c4e58d3c-574a-4050-b4a9-163736bd90cd' alt='Logo Dental Care' style='border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:142px' width='142' class='CToWUd'>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                                            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'>
                                                <tbody>
                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                                        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit' height='48'>
                                                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                                                        </td>
                                                    </tr>
                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                                                            <table cellpadding='0' cellspacing='0' style='margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse' width='100%'>
                                                                <tbody>
                                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;height:44px;width:100%;border-collapse:collapse;font-family:inherit'>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'></td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit' width='44' height='44'>
                                                                            <a href='https://www.facebook.com/profile.php?id=100093052218614' style='color:#bd2225;text-decoration:underline' target='_blank'>
                                                                                <img alt='Biểu tượng Facebook' src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ffacebookmail.png?alt=media&token=f88279fd-f9ad-455a-a093-5cd854e0e440' style='border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px' width='44' height='44' class='CToWUd'>
                                                                            </a>
                                                                        </td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit' width='24' height='44'></td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit' width='44' height='44'>
                                                                            <a href='https://www.facebook.com/profile.php?id=100093052218614' style='color:#bd2225;text-decoration:underline' target='_blank'>
                                                                                <img alt='Biểu tượng Instagram' src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Finstagrammail.png?alt=media&token=8bb0b106-6666-461a-9f94-df6b4d8872fb' style='border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px' width='44' height='44' class='CToWUd'>
                                                                            </a>
                                                                        </td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit' width='24' height='44'></td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit' width='44' height='44'>
                                                                            <a href='https://www.facebook.com/profile.php?id=100093052218614' style='color:#bd2225;text-decoration:underline' target='_blank'>
                                                                                <img alt='Biểu tượng YouTube' src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Fyoutubemail.png?alt=media&token=725229c6-d9a8-4635-95fb-3383df77de83' style='border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px' width='44' height='44' class='CToWUd'>
                                                                            </a>
                                                                        </td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit' width='24' height='44'></td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit' width='44' height='44'>
                                                                            <a href='https://www.facebook.com/profile.php?id=100093052218614' style='color:#bd2225;text-decoration:underline' target='_blank'>
                                                                                <img alt='Biểu tượng Twitter' src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Logo%2Ftwittermail.png?alt=media&token=b86058d2-11ce-438f-aa25-915dbfb1c1bb' style='border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px' width='44' height='44' class='CToWUd'>
                                                                            </a>
                                                                        </td>
                                                                        <td style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                                                        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:32px;border-collapse:collapse;font-family:inherit' height='32'>
                                                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit' width='72' height='100%'>
                            <div style='height:100%;overflow:hidden;width:72px;font-family:inherit'></div>
                        </td>
                    </tr>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td colspan='3' style='margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit' height='64'>
                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <table cellpadding='0' cellspacing='0' style='padding:0;border:none;border-spacing:0;width:100%;margin:0 auto;border-collapse:collapse' width='100%'>
                <tbody>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
                            <span style='display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit'>
                                <a href='#' style='text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle' target='_blank'>Chính sách Quyền riêng tư</a>
                            </span>
                            <span style='display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit'>
                                <img src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670' style='border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px' width='4' class='CToWUd'>
                                <a href='#' style='text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle' target='_blank'>Hỗ trợ</a>
                            </span>
                            <span style='display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit'>
                                <img src='https://firebasestorage.googleapis.com/v0/b/auth-demo-123e3.appspot.com/o/Dental%20Care%20Icons%2Fdot-icon.png?alt=media&token=9f8a244b-64cf-4c4b-89c7-8dcaa6f42670' style='border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px' width='4' class='CToWUd'>
                                <a href='#' style='text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle' target='_blank'>Điều khoản Sử dụng</a>
                            </span>
                        </td>
                    </tr>
                    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
                        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit' height='16'>
                            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <span style='font-family:inherit'>Đây là dịch vụ thư thông báo.</span>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit' height='16'>
            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <span style='font-family:inherit'>Tập Đoàn Dental Care, Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 © 2024 Dental Care. Đã đăng ký bản quyền</span>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td colspan='1' style='margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit' height='16'>
            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td style='margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit' align='center'>
            <span style='font-family:inherit'>© năm 2024 bởi Tập Đoàn Dental Care, Dental Care, Nền tảng đặt lịch nha sĩ và các logo liên quan là nhãn hiệu, nhãn hiệu dịch vụ và/hoặc nhãn hiệu đã đăng ký của Tập Đoàn Dental Care.</span>
        </td>
    </tr>
    <tr style='margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit'>
        <td colspan='3' style='margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit' height='64'>
            <table style='margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse' width='100%'></table>
        </td>
    </tr>
</tbody>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(order.CompanyEmail);

                // Optional: Add additional headers to improve deliverability
                mailMessage.Headers.Add("X-Priority", "1");
                mailMessage.Headers.Add("X-MSMail-Priority", "High");
                mailMessage.Headers.Add("Importance", "High");

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {order.CompanyEmail} with {subject}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"Email did not send to {order.CompanyEmail}");
                throw; // Re-throw the exception if you want the caller to handle it
            }
        }
    }
}
