using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authentication;

namespace Dental_Clinic_System.Services.GoogleSecurity
{
    public class GoogleSecurity
    {
        private readonly IConfiguration _configuration;
        private readonly PeopleServiceService _peopleService;

        public GoogleSecurity(IConfiguration configuration)
        {
            _configuration = configuration;
            var initializer = new BaseClientService.Initializer()
            {
                ApiKey = _configuration["Google:ApiKey"]
            };
            _peopleService = new PeopleServiceService(initializer);
        }

        public async Task<Person> GetUserInfoAsync(string accessToken)
        {
            var request = _peopleService.People.Get("people/me");
            request.OauthToken = accessToken;
            request.PersonFields = "emailAddresses,phoneNumbers";
            return await request.ExecuteAsync();
        }

        public async Task<bool> HasPhoneNumberLinkedAsync(string accessToken)
        {
            var userInfo = await GetUserInfoAsync(accessToken);
            return userInfo.PhoneNumbers?.Any() == true;
        }
    }
}
