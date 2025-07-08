using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Dentist_Specialty")]
    public partial class DentistSpecialty
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("SpecialtyID")]
        public int SpecialtyID { get; set; }

        [Column("DentistID")]
        public int DentistID { get; set; }
		//---------------
		[Column("Check", TypeName = "bit")]
		public bool? Check { get; set; }
		//----------

		#region Entity Mapping

		[ForeignKey("DentistID")]
        [InverseProperty("DentistSpecialties")]
        public virtual Dentist Dentist { get; set; }

        [ForeignKey("SpecialtyID")]
        [InverseProperty("DentistSpecialties")]
        public virtual Specialty Specialty { get; set; }

		#endregion
	}
}