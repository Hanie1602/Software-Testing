using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Review")]
    public partial class Review
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("DentistID")]
        public int DentistID { get; set; }

        [Column("PatientID")]
        public int PatientID { get; set; }

        [Column("Rating")]
        public double? Rating { get; set; } = null;

        [Column("Comment",TypeName = "nvarchar(2000)")]
        public string Comment { get; set; } = null!;

        [Column("Date",TypeName ="DATE")]
        public DateOnly Date { get; set; }

		#region Foreign Key

		[ForeignKey("DentistID")]
        [InverseProperty("Reviews")]
        public virtual Dentist? Dentist { get; set; }

        [ForeignKey("PatientID")]
        [InverseProperty("Reviews")]

		#endregion

		#region Entity Mapping

		public virtual Account Patient { get; set; } = null!;

		#endregion 
	}
}