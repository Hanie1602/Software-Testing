using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dental_Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class MigrationLinaV50 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Check",
                table: "Dentist_Specialty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AmWorkTime",
                table: "Clinic",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PmWorkTime",
                table: "Clinic",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekDay = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    SessionInDay = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorkTimes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Session = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time(7)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTime", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Dentist_Sessions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Session_ID = table.Column<int>(type: "int", nullable: false),
                    Dentist_ID = table.Column<int>(type: "int", nullable: false),
                    Check = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dentist_Session", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Dentist__Dentist_Session",
                        column: x => x.Dentist_ID,
                        principalTable: "Dentist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Dentist__Session_Dentist",
                        column: x => x.Session_ID,
                        principalTable: "Sessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clinic_AmWorkTime",
                table: "Clinic",
                column: "AmWorkTime");

            migrationBuilder.CreateIndex(
                name: "IX_Clinic_PmWorkTime",
                table: "Clinic",
                column: "PmWorkTime");

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_Sessions_Dentist_ID",
                table: "Dentist_Sessions",
                column: "Dentist_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_Sessions_Session_ID",
                table: "Dentist_Sessions",
                column: "Session_ID");

            migrationBuilder.AddForeignKey(
                name: "FK__AmWorkTime__Clinic",
                table: "Clinic",
                column: "AmWorkTime",
                principalTable: "WorkTimes",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__PmWorkTime__Clinic",
                table: "Clinic",
                column: "PmWorkTime",
                principalTable: "WorkTimes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__AmWorkTime__Clinic",
                table: "Clinic");

            migrationBuilder.DropForeignKey(
                name: "FK__PmWorkTime__Clinic",
                table: "Clinic");

            migrationBuilder.DropTable(
                name: "Dentist_Sessions");

            migrationBuilder.DropTable(
                name: "WorkTimes");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Clinic_AmWorkTime",
                table: "Clinic");

            migrationBuilder.DropIndex(
                name: "IX_Clinic_PmWorkTime",
                table: "Clinic");

            migrationBuilder.DropColumn(
                name: "Check",
                table: "Dentist_Specialty");

            migrationBuilder.DropColumn(
                name: "AmWorkTime",
                table: "Clinic");

            migrationBuilder.DropColumn(
                name: "PmWorkTime",
                table: "Clinic");
        }
    }
}
