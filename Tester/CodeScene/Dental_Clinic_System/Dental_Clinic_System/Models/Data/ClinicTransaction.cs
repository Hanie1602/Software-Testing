using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Models.Data
{
    public class ClinicTransaction
    {
        [Key]
        public int ID { get; set; }

        [Column("Wallet_ID", TypeName = "int")]
        public int Wallet_ID { get; set; }

        [Column("Date", TypeName = "date")]
        public DateTime Date { get; set; }

        [Column("TransactionCode", TypeName = "int")]
        public int TransactionCode { get; set; }

        [Column("PaymentMethod", TypeName = "nvarchar(50)")]
        public string? PaymentMethod { get; set; }

        [Column("Description", TypeName = "nvarchar(1000)")]
        public string? Description { get; set; }

        [Column("Deposit", TypeName = "money")]
        public decimal Deposit { get; set; }

        [Column("ClinicTransactionStatus", TypeName = "nvarchar(50)")]
        public string ClinicTransactionStatus { get; set; } = null!;

        [Column("Bank", TypeName = "nvarchar(50)")]
        public string? Bank { get; set; }

        #region Foreign Key
        [ForeignKey("Wallet_ID")]
        [InverseProperty("ClinicTransactions")]
        public virtual Wallet Wallet { get; set; } = null!;
        #endregion
    }
}
