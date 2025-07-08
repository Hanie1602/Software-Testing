using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Specialty")]
    public partial class Specialty
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Name",TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;

        [Column("Description",TypeName = "ntext")]
        public string? Description { get; set; }

        [Column("Image", TypeName = "varchar(256)")]
        public string? Image { get; set; }

        [Column("Deposit", TypeName = "money")]
        public  decimal Deposit {  get; set; }

		#region Entity Mapping

		[InverseProperty("Specialty")]
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        [InverseProperty("Specialty")]
        public virtual ICollection<DentistSpecialty> DentistSpecialties { get; set; } = new List<DentistSpecialty>();

        [InverseProperty("Specialty")]
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();

		#endregion
	}
}