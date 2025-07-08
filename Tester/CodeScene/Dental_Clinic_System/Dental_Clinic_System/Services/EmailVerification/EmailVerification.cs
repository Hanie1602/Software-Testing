using ZeroBounceSDK;

namespace Dental_Clinic_System.Services.EmailVerification
{
    public class EmailVerification : IEmailVerification
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailVerification> _logger;

        public EmailVerification(IConfiguration configuration, ILogger<EmailVerification> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var apiKey = _configuration["ZeroBounce:ApiKey"];
            ZeroBounce.Instance.Initialize(apiKey);
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            var success = false;
            ZeroBounce.Instance.Validate(email, null, response =>
            {
                if (response.Status == ZBValidateStatus.Valid)
                {
                    _logger.LogInformation($"Email {email} is valid.");
                    success = true;
                }
                else
                {
                    _logger.LogWarning($"Email {email} is not valid. Status: {response.Status}");
                }
            }, error =>
            {
                _logger.LogError($"Error validating email {email}: {error}");
            });

            return success;
        }
    }
}
