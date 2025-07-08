using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dental_Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class MigrationSoyuV44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Valid_PatientRecord_Status",
                table: "PatientRecord");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Valid_PatientRecord_Status",
                table: "PatientRecord",
                sql: "PatientRecordStatus = N'Đã Xóa' OR PatientRecordStatus = N'Đang Tồn Tại'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Valid_PatientRecord_Status",
                table: "PatientRecord");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Valid_PatientRecord_Status",
                table: "PatientRecord",
                sql: "PatientRecordStatus = 'Đã Xóa' OR PatientRecordStatus = 'Đang Tồn Tại'");
        }
    }
}
