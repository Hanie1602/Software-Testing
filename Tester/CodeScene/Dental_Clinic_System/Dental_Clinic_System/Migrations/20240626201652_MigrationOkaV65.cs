using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dental_Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class MigrationOkaV65 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Appointment__Schedule",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ScheduleID",
                table: "Appointment");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ScheduleID",
                table: "Appointment",
                column: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedule__Appointments",
                table: "Appointment",
                column: "ScheduleID",
                principalTable: "Schedule",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Schedule__Appointments",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ScheduleID",
                table: "Appointment");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ScheduleID",
                table: "Appointment",
                column: "ScheduleID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Appointment__Schedule",
                table: "Appointment",
                column: "ScheduleID",
                principalTable: "Schedule",
                principalColumn: "ID");
        }
    }
}
