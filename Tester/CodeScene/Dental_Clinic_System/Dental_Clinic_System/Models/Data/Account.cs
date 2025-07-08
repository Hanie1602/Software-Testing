using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Models.Data
{
	[Table("Account")]
	public partial class Account
	{
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		[Column("Username", TypeName = "varchar(30)")]
		public string Username { get; set; } = null!;

		[Column("Password", TypeName = "varchar(30)")]
		public string Password { get; set; } = null!;

		[Column("Role", TypeName = "nvarchar(9)")]
		public string Role { get; set; } = null!;

		[Column("Gender", TypeName = "nvarchar(6)")]
		public string? Gender { get; set; } = null;

		[Column("FirstName", TypeName = "nvarchar(50)")]
		public string? FirstName { get; set; } = null;

		[Column("LastName", TypeName = "nvarchar(50)")]
		public string? LastName { get; set; } = null;

		[Column("Email", TypeName = "varchar(50)")]
		public string? Email { get; set; }

		[Column("PhoneNumber", TypeName = "varchar(11)")]
		public string? PhoneNumber { get; set; }

		[Column("DateOfBirth", TypeName = "DATE")]
		public DateOnly? DateOfBirth { get; set; }

		[Column("Province")]
		public int? Province {  get; set; } = null;

		[Column("Ward")]
		public int? Ward { get; set; } = null;

		[Column("District")]
		public int? District { get; set; } = null;

		[Column("Address", TypeName = "nvarchar(100)")]
		public string? Address { get; set; } = null;

		//[Column("Rating")]
		//public double? Rating { get; set; } = null;

        [Column("Image", TypeName = "varchar(256)")]
		public string? Image { get; set; } = null;

		[Column("IsLinked")]
		public bool? IsLinked { get; set; } = false;

		[Column("AccountStatus", TypeName = "nvarchar(30)")]
		public string AccountStatus { get; set; } = null!;


		#region Entity Mapping
		[InverseProperty("Manager")]
		public virtual Clinic? Clinics { get; set; }

		[InverseProperty("Account")]
		public virtual Dentist? Dentists { get; set; }

		[InverseProperty("Account")]
		public virtual ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();

		[InverseProperty("Patient")]
		public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

		[InverseProperty("Account")]
		public virtual Wallet? Wallet { get; set; }

		[InverseProperty("Account")]
		public virtual ICollection<News> News { get; set; } = new List<News>();
        #endregion
    }
}