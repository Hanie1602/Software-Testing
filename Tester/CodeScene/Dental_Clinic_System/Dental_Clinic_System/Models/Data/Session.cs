using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Models.Data
{
	public class Session
	{
		[Key]
		public int ID { get; set; }

		[Column("WeekDay", TypeName = "nvarchar(20)")]
		public string WeekDay { get; set; }

		[Column("SessionInDay", TypeName = "nvarchar(20)")]
		public string SessionInDay { get; set; }


		[InverseProperty("Session")]
		public virtual ICollection<Dentist_Session> SessionsDentist { get; set; } = new List<Dentist_Session>();
	}
}
