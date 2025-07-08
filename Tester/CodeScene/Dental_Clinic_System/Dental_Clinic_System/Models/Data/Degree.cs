using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Degree")]
    public partial class Degree
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

		#region Entity Mapping

		[InverseProperty("Degree")]
        public virtual ICollection<Dentist> Dentists { get; set; } = new List<Dentist>();

		#endregion
	}
}