using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Province")]
    public partial class Province
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [StringLength(20)]
        public string Name { get; set; } = null!;

        [InverseProperty("Province")]
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

        [InverseProperty("Province")]
        public virtual ICollection<Clinic> Clinics { get; set; } = new List<Clinic>();
    }
}
