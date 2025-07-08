using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("TimeSlot")]
    public partial class TimeSlot
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("StartTime", TypeName = "time(7)")]
        public TimeOnly StartTime { get; set; }

		[Column("EndTime", TypeName = "time(7)")]
		public TimeOnly EndTime { get; set; }

		#region Entity Mapping

		[InverseProperty("TimeSlot")]
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

		#endregion
	}
}