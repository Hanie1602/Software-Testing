using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Schedule")]
    public partial class Schedule
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("DentistID")]
        public int DentistID { get; set; }

        [Column("TimeSlotID")]
        public int TimeSlotID { get; set; }

        [Column("Date", TypeName = "DATE")]
        public DateOnly Date { get; set; }

        [Column("ScheduleStatus",TypeName = "nvarchar(30)")]
        public string ScheduleStatus { get; set; } = null!;

        #region Entity Mapping

        // Update to reflect a one-to-many relationship with Appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        [ForeignKey("DentistID")]
        [InverseProperty("Schedules")]
        public virtual Dentist Dentist { get; set; } = null!;

        [ForeignKey("TimeSlotID")]
        [InverseProperty("Schedules")]
        public virtual TimeSlot TimeSlot { get; set; } = null!;

		#endregion
	}
}