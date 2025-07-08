using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Clinic_System.Models.Data
{
    [Table("News")]
    public partial class News
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("Date", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column("Title", TypeName = "nvarchar(250)")]
        public string? Title { get; set; }

        [Column("Content", TypeName = "ntext")]
        public string? Content { get; set; }

        [Column("ThumbNail", TypeName = "varchar(500)")]
        public string? ThumbNail {  get; set; } = null;

        [Column("Status", TypeName = "nvarchar(50)")]
        public string? Status { get; set; } = null;

        #region Foreign Key
        [ForeignKey("AccountID")]
        [InverseProperty("News")]
        public virtual Account Account { get; set; } = null!;

        #endregion
    }
}
