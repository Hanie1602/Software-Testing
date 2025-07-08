using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Models.Data
{
    [Table("Transaction")]
    public partial class Transaction
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("AppointmentID")]
        public int AppointmentID { get; set; }

        [Column("Date", TypeName = "DATETIME")]
        public DateTime Date { get; set; }

        [Column("BankAccountNumber",TypeName = "varchar(20)")]
        public string BankAccountNumber { get; set; } = null!;

        [Column("BankName", TypeName ="nvarchar(100)")]
        public string BankName { get; set; } = null!;

        [Column("TransactionCode", TypeName = "varchar(50)")]
        public string? TransactionCode { get; set; }

		[Column("PaymentMethod", TypeName = "varchar(50)")]
		public string? PaymentMethod { get; set; }

		[Column("MedicalReportID", TypeName = "varchar(50)")]
		public string MedicalReportID { get; set; } = Helper.DepositIDGenerator.GenerateDepositID();

		[Column("TotalPrice", TypeName = "money")]
		public decimal? TotalPrice { get; set; }

		[Column("Status", TypeName = "nvarchar(50)")]
		public string? Status { get; set; }

		[Column("FullName", TypeName = "nvarchar(50)")]
		public string? FullName { get; set; }

        [Column("Message", TypeName = "nvarchar(100)")]
        public string? Message { get; set; } = null;

        #region Foreign Key

        [ForeignKey("AppointmentID")]
        [InverseProperty("Transactions")]

		#endregion

		#region Entity Mapping

		public virtual Appointment Appointment { get; set; }

		#endregion
	}
}