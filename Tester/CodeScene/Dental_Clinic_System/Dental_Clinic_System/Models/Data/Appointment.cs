using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Appointment")]
    public partial class Appointment
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

		[Column("ScheduleID")]
        public int ScheduleID { get; set; }

        [Column("PatientRecordID")]
        public int PatientRecordID { get; set; }

        [Column("SpecialtyID")]
        public int SpecialtyID { get; set; }

        [Column("AppointmentStatus", TypeName ="nvarchar(30)")]
        public string AppointmentStatus { get; set; } = null!;

		[Column("TotalPrice",TypeName = "money")]
        public decimal TotalPrice { get; set; }

        [Column("Description", TypeName = "nvarchar(1000)")]
        public string? Description { get; set; }

        [Column("CreatedDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column("IsRated")]
        public string? IsRated { get; set; } = null;
		#region Foreign Key

		[ForeignKey("PatientRecordID")]
        [InverseProperty("Appointments")]
        public virtual PatientRecord PatientRecords { get; set; } = null!;

        [ForeignKey("ScheduleID")]
        [InverseProperty("Appointments")]
        public virtual Schedule Schedule { get; set; } = null!;

        [ForeignKey("SpecialtyID")]
        [InverseProperty("Appointments")]
        public virtual Specialty Specialty { get; set; } = null!;

		#endregion

		#region Entity Mapping

		[InverseProperty("Appointment")]
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();



		#endregion
	}
}