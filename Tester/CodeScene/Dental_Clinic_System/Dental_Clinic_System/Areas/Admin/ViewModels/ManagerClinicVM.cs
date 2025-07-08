using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Admin.ViewModels
{
    public class ManagerClinicVM
    {
		public int Id { get; set; }

		public string ClinicName { get; set; }

		public string? Address { get; set; }

        public string ManagerName { get; set; }


		public string Name { get; set; }

		public string? Description { get; set; }

		public string Image { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }

		public string Basis { get; set; }

		public string? ClinicStatus { get; set; }

		//public string MapLinker { get; set; }

		public int ManagerID { get; set; }

		public int? Province { get; set; }

		public int? District { get; set; }

		public int? Ward { get; set; }

	}
}
