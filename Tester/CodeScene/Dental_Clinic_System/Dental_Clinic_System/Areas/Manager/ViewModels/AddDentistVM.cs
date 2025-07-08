using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Manager.ViewModels
{
	public class AddDentistVM
	{
		public List<int> SpecialtyIDs { get; set; } = new List<int>();
		public string Username { get; set; }

		public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Gender { get; set; }

        public string Email { get; set; } = null!;

		public string PhoneNumber { get; set; }
        public int DegreeID { get; set; }
        public string? Description { get; set; }


    }
}
