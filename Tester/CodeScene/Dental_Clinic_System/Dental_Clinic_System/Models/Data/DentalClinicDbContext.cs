using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dental_Clinic_System.Models.Data
{
	public partial class DentalClinicDbContext : DbContext
	{
		public DentalClinicDbContext() { }

		public DentalClinicDbContext(DbContextOptions<DentalClinicDbContext> options) : base(options) { }

		#region DbSet
		public virtual DbSet<Account> Accounts { get; set; }
		public virtual DbSet<Appointment> Appointments { get; set; }
		public virtual DbSet<Clinic> Clinics { get; set; }
		public virtual DbSet<Degree> Degrees { get; set; }
		public virtual DbSet<Dentist> Dentists { get; set; }
		public virtual DbSet<DentistSpecialty> DentistSpecialties { get; set; }
		public virtual DbSet<PatientRecord> PatientRecords { get; set; }
		public virtual DbSet<Review> Reviews { get; set; }
		public virtual DbSet<Schedule> Schedules { get; set; }
		public virtual DbSet<Service> Services { get; set; }
		public virtual DbSet<Specialty> Specialties { get; set; }
		public virtual DbSet<TimeSlot> TimeSlots { get; set; }
		public virtual DbSet<Transaction> Transactions { get; set; }
		public virtual DbSet<Wallet> Wallets { get; set; }
		public virtual DbSet<ClinicTransaction> ClinicTransactions { get; set; }
		public virtual DbSet<News> News { get; set; }
		public virtual DbSet<Order> Orders { get; set; }

		//================================================================================================================================
		public virtual DbSet<Dentist_Session> Dentist_Sessions { get; set; }
		public virtual DbSet<Session> Sessions { get; set; }

		public virtual DbSet<WorkTime> WorkTimes { get; set; }
		//================================================================================================================================
		#endregion

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Name=DBConnection");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Account");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasIndex(e => e.Username).IsUnique();

				entity.Property(e => e.PhoneNumber).IsFixedLength();

				entity.HasIndex(e => e.PhoneNumber).IsUnique();

				entity.HasIndex(e => e.Email).IsUnique();

				entity.Property(e => e.Role).IsRequired();

				entity.Property(e => e.AccountStatus).IsRequired();


				entity.HasCheckConstraint("CK_Valid_Role", "[Role] = 'Admin' OR [Role] = 'PatientRecord' OR [Role] = 'Dentist' OR [Role] = 'Manager' OR [Role] = N'Bệnh Nhân' OR [Role] = N'Nha Sĩ' OR [Role] = N'Quản Lý'");

				entity.HasCheckConstraint("CK_Valid_Gender", "[Gender] = 'Male' OR [Gender] = 'Female' OR [Gender] = N'Nam' OR [Gender] = N'Nữ'");

				entity.HasCheckConstraint("CK_Valid_Status_Account", "[AccountStatus] = 'Active' OR [AccountStatus] = 'Banned' OR [AccountStatus] = N'Not Active' OR [AccountStatus] = N'Hoạt Động' OR [AccountStatus] = N'Bị Khóa' OR [AccountStatus] = N'Chưa Kích Hoạt'");
			});

			modelBuilder.Entity<Appointment>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Appointment");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasCheckConstraint("CK_Valid_Status_Appointment", "[AppointmentStatus] = 'Pending' OR [AppointmentStatus] = 'Canceled' OR [AppointmentStatus] = 'Approved' OR [AppointmentStatus] = 'Completed' OR [AppointmentStatus] = N'Chờ Xác Nhận' OR [AppointmentStatus] = N'Đã Hủy' OR [AppointmentStatus] = N'Đã Chấp Nhận' OR [AppointmentStatus] = N'Đã Khám'");

				entity.HasOne(d => d.PatientRecords).WithMany(p => p.Appointments).IsRequired(false).HasConstraintName("FK__Appointment__PatientRecord").OnDelete(DeleteBehavior.NoAction);

                // Remove the unique constraint on ScheduleID
                entity.HasIndex(e => e.ScheduleID).IsUnique(false);

                //entity.HasOne(d => d.Schedule).WithOne(p => p.Appointments).IsRequired(true).HasConstraintName("FK__Appointment__Schedule").OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.Schedule)
        .WithMany(p => p.Appointments) // Updated to WithMany to allow multiple appointments per schedule
        .IsRequired(true)
        .HasConstraintName("FK__Appointment__Schedule")
        .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.Specialty).WithMany(p => p.Appointments).HasConstraintName("FK__Appointment__Specialty");
			});

			modelBuilder.Entity<Clinic>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Clinic");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				//entity.ToTable("Clinic", tb => tb.HasTrigger("trg_Clinic_Manager_Insert"));

				entity.HasOne(d => d.Manager).WithOne(p => p.Clinics).IsRequired(false).HasConstraintName("FK__Clinic__Manager");

			});

			modelBuilder.Entity<Degree>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Degree");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<Dentist>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Dentist");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasOne(d => d.Account).WithOne(p => p.Dentists).IsRequired(false).HasConstraintName("FK__Dentist__Account").OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.Clinic).WithMany(p => p.Dentists).HasConstraintName("FK__Dentist__Clinic").OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.Degree).WithMany(p => p.Dentists).HasConstraintName("FK__Dentist__Degree").OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<DentistSpecialty>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_DentistSpecialty");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasOne(d => d.Dentist).WithMany(p => p.DentistSpecialties).HasConstraintName("FK__DentistSpecialty__Dentist").OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.Specialty).WithMany(p => p.DentistSpecialties).HasConstraintName("FK__DentistSpecialty__Specialty").OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<PatientRecord>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Patient");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.Property(e => e.MemberCard).IsFixedLength();

				entity.HasIndex(e => e.MemberCard).IsUnique();

				entity.HasOne(d => d.Account).WithMany(p => p.PatientRecords).IsRequired(false).HasConstraintName("FK__Patient__Account").OnDelete(DeleteBehavior.Cascade);

				entity.HasCheckConstraint("CK_Valid_PatientRecord_Status", "PatientRecordStatus = N'Đã Xóa' OR PatientRecordStatus = N'Đang Tồn Tại'");
			});

			modelBuilder.Entity<Review>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Review");
				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasOne(d => d.Dentist).WithMany(p => p.Reviews).HasConstraintName("FK__Review__Dentist").OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.Patient).WithMany(p => p.Reviews).HasConstraintName("FK__Review__Patient").OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Schedule>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Schedule");
				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasOne(d => d.Dentist).WithMany(p => p.Schedules).HasConstraintName("FK__Schedule__Dentis").OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.TimeSlot).WithMany(p => p.Schedules).HasConstraintName("FK__Schedule__TimeSlot").OnDelete(DeleteBehavior.Restrict);

                // Updated to reflect the one-to-many relationship with Appointments
                entity.HasMany(d => d.Appointments)
                    .WithOne(p => p.Schedule)
                    .HasForeignKey(p => p.ScheduleID)
                    .HasConstraintName("FK__Schedule__Appointments")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasCheckConstraint("CK_Valid_Schedule_Status", "ScheduleStatus = 'Booked' OR ScheduleStatus = 'Available' OR ScheduleStatus = N'Đã Đặt' OR ScheduleStatus = N'Còn Trống'");
			});

			modelBuilder.Entity<Service>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Service");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasIndex(e => e.Name).IsUnique();

				entity.HasOne(d => d.Clinic).WithMany(p => p.Services).HasConstraintName("FK__Service__Clinic").OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.Specialty).WithMany(p => p.Services).HasConstraintName("FK__Service__Special").OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Specialty>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Specialty");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<TimeSlot>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_TimeSlot");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();
			});

			modelBuilder.Entity<Transaction>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Transaction");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.Property(e => e.BankAccountNumber).IsFixedLength();

				entity.HasOne(d => d.Appointment).WithMany(p => p.Transactions).HasConstraintName("FK__Transacti__Appointment").OnDelete(DeleteBehavior.NoAction);
			});

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.ID).HasName("PK_Wallet");

                entity.Property(e => e.ID).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Account).WithOne(p => p.Wallet).HasConstraintName("FK__Wallet__Account").OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ClinicTransaction>(entity =>
            {
                entity.HasKey(e => e.ID).HasName("PK_ClinicTransaction");

                entity.Property(e => e.ID).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Wallet).WithMany(p => p.ClinicTransactions).HasConstraintName("FK__CLinicTrans__Wallet").OnDelete(DeleteBehavior.Restrict);

				//entity.HasCheckConstraint("CK_Valid_ClinicTransaction_Status", "ClinicTransactionStatus = 'Pending' OR ClinicTransactionStatus = 'Completed' OR ClinicTransactionStatus = 'Canceled' OR ClinicTransactionStatus = N'Chờ Xác Nhận' OR ClinicTransactionStatus = N'Đã Thanh Toán' OR ClinicTransactionStatus = N'Đã Hủy'");
            });

			modelBuilder.Entity<News>(entity =>
			{
                entity.HasKey(e => e.ID).HasName("PK_News");

                entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasOne(d => d.Account).WithMany(p => p.News).HasConstraintName("FK__News__Account").OnDelete(DeleteBehavior.NoAction);
            });

			modelBuilder.Entity<Order>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Order");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasCheckConstraint("CK_CHECKVALID_STATUS", "Status = N'Chưa Duyệt' OR Status = N'Từ Chối' OR Status = N'Đồng Ý'");
			});

			modelBuilder.Entity<Dentist_Session>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Dentist_Session");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasOne(d => d.Dentist).WithMany(p => p.DentistSessions).HasConstraintName("FK__Dentist__Dentist_Session").OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.Session).WithMany(p => p.SessionsDentist).HasConstraintName("FK__Dentist__Session_Dentist").OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Session>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_Session");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();
			});

			modelBuilder.Entity<WorkTime>(entity =>
			{
				entity.HasKey(e => e.ID).HasName("PK_WorkTime");

				entity.Property(e => e.ID).ValueGeneratedOnAdd();

				entity.HasMany(d => d.AmWorkTimeClinics).WithOne(p => p.AmWorkTimes).HasForeignKey(fk => fk.AmWorkTimeID).HasConstraintName("FK__AmWorkTime__Clinic").IsRequired(false);

				entity.HasMany(d => d.PmWorkTimeClinics).WithOne(p => p.PmWorkTimes).HasForeignKey(fk => fk.PmWorkTimeID).HasConstraintName("FK__PmWorkTime__Clinic").IsRequired(false);
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}