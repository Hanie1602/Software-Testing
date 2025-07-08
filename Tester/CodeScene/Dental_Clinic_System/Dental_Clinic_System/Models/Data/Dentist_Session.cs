using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Models.Data
{
	public class Dentist_Session
	{
		[Key]
		public int ID { get; set; }

		[Column("Session_ID", TypeName = "int")]
		public int Session_ID { get; set; }
		[Column("Dentist_ID", TypeName = "int")]
		public int Dentist_ID { get; set; }
		//-------------
		[Column("Check", TypeName = "bit")]
		public bool? Check { get; set; }
		//-------------

		[ForeignKey("Session_ID")]
		[InverseProperty("SessionsDentist")]
		public virtual Session Session { get; set; }

		[ForeignKey("Dentist_ID")]
		[InverseProperty("DentistSessions")]
		public virtual Dentist Dentist { get; set; }
	}
}
