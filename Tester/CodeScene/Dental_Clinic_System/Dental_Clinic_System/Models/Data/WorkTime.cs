using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Models.Data
{
	public class WorkTime
	{
		[Key]
		public int ID { get; set; }

		[Column("Session", TypeName = "nvarchar(50)")]
		public string Session { get; set; }

		[Column("StartTime", TypeName = "time(7)")]
		public TimeOnly StartTime { get; set; }

		[Column("EndTime", TypeName = "time(7)")]
		public TimeOnly EndTime { get; set;}


		[InverseProperty("AmWorkTimes")]
		public virtual ICollection<Clinic> AmWorkTimeClinics { get; set; } = new List<Clinic>();

		// Thuộc tính cho quan hệ với Clinic cho PM
		[InverseProperty("PmWorkTimes")]
		public virtual ICollection<Clinic> PmWorkTimeClinics { get; set; } = new List<Clinic>();
	}
}
