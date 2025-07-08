using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Patient")]
    public partial class Patient
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("AccountID")]
        public int? AccountID { get; set; }

        [StringLength(15)]
        [Unicode(false)]
        public string? MemberCard { get; set; }

        [ForeignKey("AccountID")]
        [InverseProperty("Patients")]
        public virtual Account? Account { get; set; }

        [InverseProperty("Patient")]
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();

        [InverseProperty("Patient")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
