using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Manager.ViewModels
{
	public class EditDentistVM
	{
		public List<int> SpecialtyIDs { get; set; } = new List<int>();
		public int DentistId { get; set; }
        public int AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Gender { get; set; }
        public int? Province { get; set; } = null!;

        public int? Ward { get; set; } = null!;

        public int? District { get; set; } = null!;

        public string? Address { get; set; } = null!;

        public DateOnly? DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

		public string PhoneNumber { get; set; }
        public int DegreeID { get; set; }
        public string? Description { get; set; }
    }
}
