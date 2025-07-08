using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dental_Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class MigrationSoyuV43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(30)", nullable: false),
                    Password = table.Column<string>(type: "varchar(30)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(9)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", fixedLength: true, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "DATE", nullable: true),
                    Province = table.Column<int>(type: "int", nullable: true),
                    Ward = table.Column<int>(type: "int", nullable: true),
                    District = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Image = table.Column<string>(type: "varchar(256)", nullable: true),
                    IsLinked = table.Column<bool>(type: "bit", nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.ID);
                    table.CheckConstraint("CK_Valid_Gender", "[Gender] = 'Male' OR [Gender] = 'Female' OR [Gender] = N'Nam' OR [Gender] = N'Nữ'");
                    table.CheckConstraint("CK_Valid_Role", "[Role] = 'Admin' OR [Role] = 'PatientRecord' OR [Role] = 'Dentist' OR [Role] = 'Manager' OR [Role] = N'Bệnh Nhân' OR [Role] = N'Nha Sĩ' OR [Role] = N'Quản Lý'");
                    table.CheckConstraint("CK_Valid_Status_Account", "[AccountStatus] = 'Active' OR [AccountStatus] = 'Banned' OR [AccountStatus] = N'Not Active' OR [AccountStatus] = N'Hoạt Động' OR [AccountStatus] = N'Bị Khóa' OR [AccountStatus] = N'Chưa Kích Hoạt'");
                });

            migrationBuilder.CreateTable(
                name: "Degree",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degree", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Specialty",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Image = table.Column<string>(type: "varchar(256)", nullable: true),
                    Deposit = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialty", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeOnly>(type: "time(7)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clinic",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Province = table.Column<int>(type: "int", nullable: true),
                    Ward = table.Column<int>(type: "int", nullable: true),
                    District = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Basis = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Image = table.Column<string>(type: "varchar(256)", nullable: false),
                    ClinicStatus = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    MapLinker = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinic", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Clinic__Manager",
                        column: x => x.ManagerID,
                        principalTable: "Account",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PatientRecord",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    MemberCard = table.Column<string>(type: "nvarchar(15)", fixedLength: true, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(75)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(6)", nullable: false),
                    Job = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "varchar(12)", nullable: true),
                    EmailReceiver = table.Column<string>(type: "varchar(50)", nullable: true),
                    Province = table.Column<int>(type: "int", nullable: false),
                    District = table.Column<int>(type: "int", nullable: false),
                    Ward = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    PatientRecordStatus = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FMName = table.Column<string>(type: "nvarchar(75)", nullable: true),
                    FMEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    FMRelationship = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    FMPhoneNumber = table.Column<string>(type: "varchar(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.ID);
                    table.CheckConstraint("CK_Valid_PatientRecord_Status", "PatientRecordStatus = 'Đã Xóa' OR PatientRecordStatus = 'Đang Tồn Tại'");
                    table.ForeignKey(
                        name: "FK__Patient__Account",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dentist",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    ClinicID = table.Column<int>(type: "int", nullable: false),
                    DegreeID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dentist", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Dentist__Account",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Dentist__Clinic",
                        column: x => x.ClinicID,
                        principalTable: "Clinic",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Dentist__Degree",
                        column: x => x.DegreeID,
                        principalTable: "Degree",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicID = table.Column<int>(type: "int", nullable: false),
                    SpecialtyID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Service__Clinic",
                        column: x => x.ClinicID,
                        principalTable: "Clinic",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Service__Special",
                        column: x => x.SpecialtyID,
                        principalTable: "Specialty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dentist_Specialty",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialtyID = table.Column<int>(type: "int", nullable: false),
                    DentistID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentistSpecialty", x => x.ID);
                    table.ForeignKey(
                        name: "FK__DentistSpecialty__Dentist",
                        column: x => x.DentistID,
                        principalTable: "Dentist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__DentistSpecialty__Specialty",
                        column: x => x.SpecialtyID,
                        principalTable: "Specialty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DentistID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    Date = table.Column<DateOnly>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Review__Dentist",
                        column: x => x.DentistID,
                        principalTable: "Dentist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Review__Patient",
                        column: x => x.PatientID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DentistID = table.Column<int>(type: "int", nullable: false),
                    TimeSlotID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "DATE", nullable: false),
                    ScheduleStatus = table.Column<string>(type: "nvarchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ID);
                    table.CheckConstraint("CK_Valid_Schedule_Status", "ScheduleStatus = 'Booked' OR ScheduleStatus = 'Available' OR ScheduleStatus = N'Đã Đặt' OR ScheduleStatus = N'Còn Trống'");
                    table.ForeignKey(
                        name: "FK__Schedule__Dentis",
                        column: x => x.DentistID,
                        principalTable: "Dentist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Schedule__TimeSlot",
                        column: x => x.TimeSlotID,
                        principalTable: "TimeSlot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleID = table.Column<int>(type: "int", nullable: false),
                    PatientRecordID = table.Column<int>(type: "int", nullable: false),
                    SpecialtyID = table.Column<int>(type: "int", nullable: false),
                    AppointmentStatus = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "money", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.ID);
                    table.CheckConstraint("CK_Valid_Status_Appointment", "[AppointmentStatus] = 'Pending' OR [AppointmentStatus] = 'Canceled' OR [AppointmentStatus] = 'Approved' OR [AppointmentStatus] = 'Completed' OR [AppointmentStatus] = N'Chờ Xác Nhận' OR [AppointmentStatus] = N'Đã Hủy' OR [AppointmentStatus] = N'Đã Chấp Nhận' OR [AppointmentStatus] = N'Đã Khám'");
                    table.ForeignKey(
                        name: "FK__Appointment__PatientRecord",
                        column: x => x.PatientRecordID,
                        principalTable: "PatientRecord",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Appointment__Schedule",
                        column: x => x.ScheduleID,
                        principalTable: "Schedule",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Appointment__Specialty",
                        column: x => x.SpecialtyID,
                        principalTable: "Specialty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "varchar(20)", fixedLength: true, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TransactionCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "varchar(50)", nullable: true),
                    MedicalReportID = table.Column<string>(type: "varchar(50)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "money", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Transacti__Appointment",
                        column: x => x.AppointmentID,
                        principalTable: "Appointment",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_PhoneNumber",
                table: "Account",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientRecordID",
                table: "Appointment",
                column: "PatientRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ScheduleID",
                table: "Appointment",
                column: "ScheduleID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_SpecialtyID",
                table: "Appointment",
                column: "SpecialtyID");

            migrationBuilder.CreateIndex(
                name: "IX_Clinic_ManagerID",
                table: "Clinic",
                column: "ManagerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Degree_Name",
                table: "Degree",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_AccountID",
                table: "Dentist",
                column: "AccountID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_ClinicID",
                table: "Dentist",
                column: "ClinicID");

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_DegreeID",
                table: "Dentist",
                column: "DegreeID");

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_Specialty_DentistID",
                table: "Dentist_Specialty",
                column: "DentistID");

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_Specialty_SpecialtyID",
                table: "Dentist_Specialty",
                column: "SpecialtyID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecord_AccountID",
                table: "PatientRecord",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecord_MemberCard",
                table: "PatientRecord",
                column: "MemberCard",
                unique: true,
                filter: "[MemberCard] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Review_DentistID",
                table: "Review",
                column: "DentistID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_PatientID",
                table: "Review",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_DentistID",
                table: "Schedule",
                column: "DentistID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TimeSlotID",
                table: "Schedule",
                column: "TimeSlotID");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ClinicID",
                table: "Service",
                column: "ClinicID");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name",
                table: "Service",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_SpecialtyID",
                table: "Service",
                column: "SpecialtyID");

            migrationBuilder.CreateIndex(
                name: "IX_Specialty_Name",
                table: "Specialty",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AppointmentID",
                table: "Transaction",
                column: "AppointmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dentist_Specialty");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "PatientRecord");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Specialty");

            migrationBuilder.DropTable(
                name: "Dentist");

            migrationBuilder.DropTable(
                name: "TimeSlot");

            migrationBuilder.DropTable(
                name: "Clinic");

            migrationBuilder.DropTable(
                name: "Degree");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
