using Dental_Clinic_System.Models.Data;
using Microsoft.Identity.Client;

namespace Dental_Clinic_System.ViewModels
{
    public class LinkWithGoogleVM
    {
        public Account AccountLinked { get; private set; }
        public AuthenticationResult AuthenticationResultLinked { get; private set; }
    }
}
