using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Service")]
    public partial class Service
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("ClinicID")]
        public int ClinicID { get; set; }

        [Column("SpecialtyID")]
        public int SpecialtyID { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "ntext")]
        public string? Description { get; set; } = null;

        [Column("Price", TypeName = "nvarchar(200)")]
        public string Price { get; set; } = null!;

		#region Foreign Key

		[ForeignKey("ClinicID")]
        [InverseProperty("Services")]
        public virtual Clinic Clinic { get; set; }

        [ForeignKey("SpecialtyID")]
        [InverseProperty("Services")]

		#endregion

		#region Entity Mapping

		public virtual Specialty Specialty { get; set; } = null!;

		#endregion
	}
}