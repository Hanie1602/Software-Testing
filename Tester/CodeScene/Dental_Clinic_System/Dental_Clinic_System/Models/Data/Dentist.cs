using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Dentist")]
    public partial class Dentist
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("ClinicID")]
        public int ClinicID { get; set; }

        [Column("DegreeID")]
        public int DegreeID { get; set; }

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

		#region Foreign Key

		[ForeignKey("AccountID")]
        [InverseProperty("Dentists")]
        public virtual Account Account { get; set; } = null!;

        [ForeignKey("ClinicID")]
        [InverseProperty("Dentists")]
        public virtual Clinic Clinic { get; set; } = null!;

        [ForeignKey("DegreeID")]
        [InverseProperty("Dentists")]
        public virtual Degree Degree { get; set; }

		#endregion

		#region Entity Mapping

		[InverseProperty("Dentist")]
        public virtual ICollection<DentistSpecialty> DentistSpecialties { get; set; } = new List<DentistSpecialty>();

        [InverseProperty("Dentist")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        [InverseProperty("Dentist")]
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

		[InverseProperty("Dentist")]
		public virtual ICollection<Dentist_Session> DentistSessions { get; set; } = new List<Dentist_Session>();
		#endregion
	}
}