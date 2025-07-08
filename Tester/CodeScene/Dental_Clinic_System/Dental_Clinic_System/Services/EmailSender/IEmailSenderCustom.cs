using Dental_Clinic_System.Models.Data;

namespace Dental_Clinic_System.Services.EmailSender
{
    public interface IEmailSenderCustom
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailForUpdatingAsync(string email, string username, string subject, string message);
        Task SendEmailUpdatedAsync(string oldEmail, string newEmail, string subject, string message);
        Task SendInvoiceEmailAsync(Appointment appointment, Transaction transaction, int clinicID, string subject);
        Task SendBusinessPartnershipsInfo(Order order, Account managerAccount, string encryptedPassword, string subject);

    }
}
