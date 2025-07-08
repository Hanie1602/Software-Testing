using Dental_Clinic_System.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Areas.Manager.ViewModels
{
	public class ScheduleVM
	{
		//      public int? ID { get; set; }
		//public int? DentistID { get; set; }
		//public int? TimeSlotID { get; set; }
		//public DateOnly? Date { get; set; }
		//public string? ScheduleStatus { get; set; } = null!;

        //public int DentistID { get; set; }
		public List<int> DentistIDs { get; set; } = new List<int>();
		public string Dates { get; set; } = null!;

		public List<int> TimeSlots { get; set; } = null!;
	}
}
