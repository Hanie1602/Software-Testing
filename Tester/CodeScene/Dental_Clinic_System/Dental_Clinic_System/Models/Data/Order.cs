using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Models.Data
{
	public class Order
	{
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		[Column("CompanyName", TypeName = "nvarchar(200)")]
		public string CompanyName { get; set; } = null!;

		[Column("CompanyPhonenumber", TypeName = "varchar(12)")]
		public string CompanyPhonenumber { get; set; } = null!;

		[Column("CompanyEmail", TypeName = "varchar(50)")]
		public string CompanyEmail { get; set; } = null!;

		[Column("RepresentativeName", TypeName = "nvarchar(200)")]
		public string RepresentativeName { get; set; } = null!;

		[Column("ClinicName", TypeName = "nvarchar(200)")]
		public string ClinicName { get; set; } = null!;

		[Column("ClinicAddress", TypeName = "nvarchar(500)")]
		public string ClinicAddress { get; set; } = null!;

		[Column("DomainName", TypeName = "varchar(200)")]
		public string? DomainName { get; set; }

		[Column("Content", TypeName = "nvarchar(2000)")]
		public string Content { get; set; } = null!;

		[Column("Status", TypeName = "nvarchar(100)")]
		public string Status { get; set; } = null!;
	}
}
