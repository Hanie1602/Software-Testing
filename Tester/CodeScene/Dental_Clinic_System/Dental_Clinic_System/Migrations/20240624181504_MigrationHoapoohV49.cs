using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dental_Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class MigrationHoapoohV49 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Clinic",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Clinic",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Content = table.Column<string>(type: "ntext", nullable: true),
                    ThumbNail = table.Column<string>(type: "varchar(500)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                    table.ForeignKey(
                        name: "FK__News__Account",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    CompanyPhonenumber = table.Column<string>(type: "varchar(12)", nullable: false),
                    CompanyEmail = table.Column<string>(type: "varchar(50)", nullable: false),
                    RepresentativeName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ClinicName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ClinicAddress = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    DomainName = table.Column<string>(type: "varchar(200)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                    table.CheckConstraint("CK_CHECKVALID_STATUS", "Status = N'Chưa Duyệt' OR Status = N'Từ Chối' OR Status = N'Đồng Ý'");
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_AccountID",
                table: "News",
                column: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Clinic");

            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Clinic");
        }
    }
}
