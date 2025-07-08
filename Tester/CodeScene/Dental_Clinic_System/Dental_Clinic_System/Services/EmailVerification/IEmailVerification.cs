namespace Dental_Clinic_System.Services.EmailVerification
{
    public interface IEmailVerification
    {
        Task<bool> VerifyEmailAsync(string email);
    }
}
