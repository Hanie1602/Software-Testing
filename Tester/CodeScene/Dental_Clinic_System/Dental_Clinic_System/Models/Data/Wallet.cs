using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dental_Clinic_System.Models.Data
{
    public class Wallet
    {
        [Key]
        public int ID { get; set; }

        [Column("Account_ID", TypeName = "int")]
        public int Account_ID { get; set; }

        [Column("Money", TypeName = "money")]
        public decimal Money { get; set; }

        #region Foreign Key
        [ForeignKey("Account_ID")]
        [InverseProperty("Wallet")]
        public virtual Account Account { get; set; }
        #endregion

        #region Entity Mapping
        [InverseProperty("Wallet")]
        public virtual ICollection<ClinicTransaction> ClinicTransactions { get; set; } = new List<ClinicTransaction>();
        #endregion
    }
}
