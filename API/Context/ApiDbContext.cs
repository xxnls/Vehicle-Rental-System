using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Models.Vehicles;
using API.Models.Other;

namespace API.Context;

public partial class ApiDbContext : IdentityDbContext<IdentityUser<int>, EmployeeRole, int>
{
    public ApiDbContext()
    {
    }

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerStatistic> CustomerStatistics { get; set; }

    public virtual DbSet<CustomerType> CustomerTypes { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }

    public virtual DbSet<EmployeeFinance> EmployeeFinances { get; set; }

    public virtual DbSet<EmployeeLeave> EmployeeLeaves { get; set; }

    public virtual DbSet<EmployeeLeaveType> EmployeeLeaveTypes { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

    public virtual DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }

    public virtual DbSet<EmployeeShiftType> EmployeeShiftTypes { get; set; }

    public virtual DbSet<EmployeeStatistic> EmployeeStatistics { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsType> NewsTypes { get; set; }

    public virtual DbSet<PostRentalReport> PostRentalReports { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    public virtual DbSet<RentalPlace> RentalPlaces { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleBrand> VehicleBrands { get; set; }

    public virtual DbSet<VehicleModel> VehicleModels { get; set; }

    public virtual DbSet<VehicleOptionalInformation> VehicleOptionalInformations { get; set; }

    public virtual DbSet<VehicleStatistic> VehicleStatistics { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=inz_veh;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("Addresses_pk");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FirstLine).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SecondLine).HasMaxLength(100);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Addresses_Countries");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("Countries_pk");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DialingCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");

            //entity.HasKey(e => e.Id).HasName("Customers_pk");

            //entity.Property(e => e.Id).HasColumnName("CustomerID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CustomerStatisticsId).HasColumnName("CustomerStatisticsID");
            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customers_Addresses");

            entity.HasOne(d => d.CustomerStatistics).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerStatisticsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customers_CustomerStatistics");

            entity.HasOne(d => d.CustomerType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customers_CustomerTypes");
        });

        modelBuilder.Entity<CustomerStatistic>(entity =>
        {
            entity.HasKey(e => e.CustomerStatisticsId).HasName("CustomerStatistics_pk");

            entity.Property(e => e.CustomerStatisticsId).HasColumnName("CustomerStatisticsID");
            entity.Property(e => e.TotalRentals).HasDefaultValue(1);
        });

        modelBuilder.Entity<CustomerType>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeId).HasName("CustomerTypes_pk");

            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CustomerType1)
                .HasMaxLength(20)
                .HasColumnName("CustomerType");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("Documents_pk");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.DocumentCategoryId).HasColumnName("DocumentCategoryID");
            entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FileName).HasMaxLength(200);
            entity.Property(e => e.FileSizeMb).HasColumnName("FileSizeMB");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByEmployeeId).HasColumnName("ModifiedByEmployeeID");
            entity.Property(e => e.OriginalFileName).HasMaxLength(200);
            entity.Property(e => e.RentalId).HasColumnName("RentalID");
            entity.Property(e => e.RentalPlaceId).HasColumnName("RentalPlaceID");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

            entity.HasOne(d => d.CreatedByEmployee).WithMany(p => p.DocumentCreatedByEmployees)
                .HasForeignKey(d => d.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Documents_Employees_CreatedByEmployeeID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Documents)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Documents_Customers");

            entity.HasOne(d => d.DocumentCategory).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Documents_DocumentCategories");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Documents_DocumentTypes");

            entity.HasOne(d => d.Employee).WithMany(p => p.DocumentEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("Documents_Employees");

            entity.HasOne(d => d.ModifiedByEmployee).WithMany(p => p.DocumentModifiedByEmployees)
                .HasForeignKey(d => d.ModifiedByEmployeeId)
                .HasConstraintName("Documents_Employees_ModifiedByEmployeeID");

            entity.HasOne(d => d.Rental).WithMany(p => p.Documents)
                .HasForeignKey(d => d.RentalId)
                .HasConstraintName("Documents_Rentals");

            entity.HasOne(d => d.RentalPlace).WithMany(p => p.Documents)
                .HasForeignKey(d => d.RentalPlaceId)
                .HasConstraintName("Documents_RentalPlaces");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Documents)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("Documents_Vehicles");
        });

        modelBuilder.Entity<DocumentCategory>(entity =>
        {
            entity.HasKey(e => e.DocumentCategoryId).HasName("DocumentCategories_pk");

            entity.Property(e => e.DocumentCategoryId).HasColumnName("DocumentCategoryID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("DocumentCategories_DocumentCategories");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("DocumentTypes_pk");

            entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.FileExtension).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaxFileSizeMb).HasColumnName("MaxFileSizeMB");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");

            //entity.HasKey(e => e.Id).HasName("Employees_pk");

            //entity.Property(e => e.Id).HasColumnName("EmployeeID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeFinancesId).HasColumnName("EmployeeFinancesID");
            entity.Property(e => e.EmployeePositionId).HasColumnName("EmployeePositionID");
            entity.Property(e => e.EmployeeStatisticsId).HasColumnName("EmployeeStatisticsID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.RentalPlaceId).HasColumnName("RentalPlaceID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

            entity.HasOne(d => d.Address).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employees_Addresses");

            entity.HasOne(d => d.EmployeeFinances).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeFinancesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employees_EmployeeFinances");

            entity.HasOne(d => d.EmployeePosition).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeePositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employees_EmployeePositions");

            entity.HasOne(d => d.EmployeeStatistics).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeStatisticsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employees_EmployeeStatistics");

            entity.HasOne(d => d.RentalPlace).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RentalPlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employees_RentalPlaces");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.InverseSupervisor)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("Employees_Employees");
        });

        modelBuilder.Entity<EmployeeAttendance>(entity =>
        {
            entity.HasKey(e => e.EmployeeAttendanceId).HasName("EmployeeAttendance_pk");

            entity.ToTable("EmployeeAttendance");

            entity.Property(e => e.EmployeeAttendanceId).HasColumnName("EmployeeAttendanceID");
            entity.Property(e => e.CheckInTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(200);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeAttendances)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeAttendance_Employees");
        });

        modelBuilder.Entity<EmployeeFinance>(entity =>
        {
            entity.HasKey(e => e.EmployeeFinancesId).HasName("EmployeeFinances_pk");

            entity.Property(e => e.EmployeeFinancesId).HasColumnName("EmployeeFinancesID");
            entity.Property(e => e.BaseSalary).HasColumnType("money");
            entity.Property(e => e.HourlyRate).HasColumnType("money");
        });

        modelBuilder.Entity<EmployeeLeave>(entity =>
        {
            entity.HasKey(e => e.EmployeeLeaveId).HasName("EmployeeLeave_pk");

            entity.ToTable("EmployeeLeave");

            entity.Property(e => e.EmployeeLeaveId).HasColumnName("EmployeeLeaveID");
            entity.Property(e => e.ApprovedById).HasColumnName("ApprovedByID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeLeaveTypeId).HasColumnName("EmployeeLeaveTypeID");
            entity.Property(e => e.Reason).HasMaxLength(300);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.EmployeeLeaveApprovedBies)
                .HasForeignKey(d => d.ApprovedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeLeave_Employees_ApprovedByID");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeLeaveEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeLeave_Employees");

            entity.HasOne(d => d.EmployeeLeaveType).WithMany(p => p.EmployeeLeaves)
                .HasForeignKey(d => d.EmployeeLeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeLeave_EmployeeLeaveTypes");
        });

        modelBuilder.Entity<EmployeeLeaveType>(entity =>
        {
            entity.HasKey(e => e.EmployeeLeaveTypeId).HasName("EmployeeLeaveTypes_pk");

            entity.Property(e => e.EmployeeLeaveTypeId).HasColumnName("EmployeeLeaveTypeID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.HasKey(e => e.EmployeePositionId).HasName("EmployeePositions_pk");

            entity.Property(e => e.EmployeePositionId).HasColumnName("EmployeePositionID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DefaultBaseSalary).HasColumnType("money");
            entity.Property(e => e.DefaultHourlyRate).HasColumnType("money");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.ToTable("EmployeeRoles");

            entity.Property(e => e.Id).HasColumnName("EmployeeRoleID");

        });

        modelBuilder.Entity<EmployeeSchedule>(entity =>
        {
            entity.HasKey(e => e.EmployeeScheduleId).HasName("EmployeeSchedules_pk");

            entity.Property(e => e.EmployeeScheduleId).HasColumnName("EmployeeScheduleID");
            entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeShiftTypeId).HasColumnName("EmployeeShiftTypeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByEmployeeId).HasColumnName("ModifiedByEmployeeID");
            entity.Property(e => e.PlaceOfWorkId).HasColumnName("PlaceOfWorkID");

            entity.HasOne(d => d.CreatedByEmployee).WithMany(p => p.EmployeeScheduleCreatedByEmployees)
                .HasForeignKey(d => d.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeSchedule_Employees_CreatedByEmployeeID");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeScheduleEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeSchedule_Employees");

            entity.HasOne(d => d.EmployeeShiftType).WithMany(p => p.EmployeeSchedules)
                .HasForeignKey(d => d.EmployeeShiftTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeSchedule_EmployeeShiftType");

            entity.HasOne(d => d.ModifiedByEmployee).WithMany(p => p.EmployeeScheduleModifiedByEmployees)
                .HasForeignKey(d => d.ModifiedByEmployeeId)
                .HasConstraintName("EmployeeSchedule_Employees_ModifiedByEmployeeID");

            entity.HasOne(d => d.PlaceOfWork).WithMany(p => p.EmployeeSchedules)
                .HasForeignKey(d => d.PlaceOfWorkId)
                .HasConstraintName("EmployeeSchedule_RentalPlaces");
        });

        modelBuilder.Entity<EmployeeShiftType>(entity =>
        {
            entity.HasKey(e => e.EmployeeShiftTypeId).HasName("EmployeeShiftTypes_pk");

            entity.Property(e => e.EmployeeShiftTypeId).HasColumnName("EmployeeShiftTypeID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<EmployeeStatistic>(entity =>
        {
            entity.HasKey(e => e.EmployeeStatisticsId).HasName("EmployeeStatistics_pk");

            entity.Property(e => e.EmployeeStatisticsId).HasColumnName("EmployeeStatisticsID");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("Locations_pk");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Gpslatitude).HasColumnName("GPSLatitude");
            entity.Property(e => e.Gpslongitude).HasColumnName("GPSLongitude");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RentalPlaceId).HasColumnName("RentalPlaceID");
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("News_pk");

            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByEmployeeId).HasColumnName("ModifiedByEmployeeID");
            entity.Property(e => e.NewsTypeId).HasColumnName("NewsTypeID");

            entity.HasOne(d => d.CreatedByEmployee).WithMany(p => p.NewsCreatedByEmployees)
                .HasForeignKey(d => d.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("News_Employees");

            entity.HasOne(d => d.Image).WithMany(p => p.News)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("News_Documents");

            entity.HasOne(d => d.ModifiedByEmployee).WithMany(p => p.NewsModifiedByEmployees)
                .HasForeignKey(d => d.ModifiedByEmployeeId)
                .HasConstraintName("News_Employees_ModifiedByEmployeeID");

            entity.HasOne(d => d.NewsType).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("News_NewsTypes");
        });

        modelBuilder.Entity<NewsType>(entity =>
        {
            entity.HasKey(e => e.NewsTypeId).HasName("NewsTypes_pk");

            entity.Property(e => e.NewsTypeId).HasColumnName("NewsTypeID");
            entity.Property(e => e.AllowImage).HasDefaultValue(true);
            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TypeName).HasMaxLength(200);
        });

        modelBuilder.Entity<PostRentalReport>(entity =>
        {
            entity.HasKey(e => e.PostRentalReportId).HasName("PostRentalReports_pk");

            entity.Property(e => e.PostRentalReportId).HasColumnName("PostRentalReportID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.InspectorEmployeeId).HasColumnName("InspectorEmployeeID");

            entity.HasOne(d => d.InspectorEmployee).WithMany(p => p.PostRentalReports)
                .HasForeignKey(d => d.InspectorEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostRentalReports_Employees");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.RentalId).HasName("Rentals_pk");

            entity.Property(e => e.RentalId).HasColumnName("RentalID");
            entity.Property(e => e.ActualCost).HasColumnType("money");
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FinishedByEmployeeId).HasColumnName("FinishedByEmployeeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PostRentalReportId).HasColumnName("PostRentalReportID");
            entity.Property(e => e.StartedByEmployeeId).HasColumnName("StartedByEmployeeID");
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rentals_Customers");

            entity.HasOne(d => d.FinishedByEmployee).WithMany(p => p.RentalFinishedByEmployees)
                .HasForeignKey(d => d.FinishedByEmployeeId)
                .HasConstraintName("Rentals_Employees");

            entity.HasOne(d => d.PostRentalReport).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.PostRentalReportId)
                .HasConstraintName("Rentals_PostRentalReports");

            entity.HasOne(d => d.StartedByEmployee).WithMany(p => p.RentalStartedByEmployees)
                .HasForeignKey(d => d.StartedByEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rentals_Employees_StartedByEmployeeID");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rentals_Vehicles");
        });

        modelBuilder.Entity<RentalPlace>(entity =>
        {
            entity.HasKey(e => e.RentalPlaceId).HasName("RentalPlaces_pk");

            entity.Property(e => e.RentalPlaceId).HasColumnName("RentalPlaceID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.Address).WithMany(p => p.RentalPlaces)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RentalPlaces_Addresses");

            entity.HasOne(d => d.Location).WithMany(p => p.RentalPlaces)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RentalPlaces_Locations");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("VehicleID");

            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            entity.Property(e => e.Color).HasMaxLength(30);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CustomDailyRate).HasColumnType("money");
            entity.Property(e => e.CustomDeposit).HasColumnType("money");
            entity.Property(e => e.CustomWeeklyRate).HasColumnType("money");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PurchasePrice).HasColumnType("money");
            entity.Property(e => e.RentalPlaceId).HasColumnName("RentalPlaceID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("OutOfService");
            entity.Property(e => e.VehicleModelId).HasColumnName("VehicleModelID");
            entity.Property(e => e.VehicleOptionalInformationId).HasColumnName("VehicleOptionalInformationID");
            entity.Property(e => e.VehicleStatisticsId).HasColumnName("VehicleStatisticsID");
            entity.Property(e => e.VehicleTypeId).HasColumnName("VehicleTypeID");
            entity.Property(e => e.Vin)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("VIN");

            entity.HasOne(d => d.Location).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_Locations");

            entity.HasOne(d => d.RentalPlace).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.RentalPlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_RentalPlaces");

            entity.HasOne(d => d.VehicleModel).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_VehicleModels");

            entity.HasOne(d => d.VehicleOptionalInformation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleOptionalInformationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_VehicleOptionalInformation");

            entity.HasOne(d => d.VehicleStatistics).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleStatisticsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_VehicleStatistics");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_VehicleTypes");
        });

        modelBuilder.Entity<VehicleBrand>(entity =>
        {
            entity.HasKey(e => e.VehicleBrandId).HasName("VehicleBrands_pk");

            entity.Property(e => e.VehicleBrandId).HasColumnName("VehicleBrandID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(200)
                .HasColumnName("LogoURL");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(200);
        });

        modelBuilder.Entity<VehicleModel>(entity =>
        {
            entity.HasKey(e => e.VehicleModelId).HasName("VehicleModels_pk");

            entity.Property(e => e.VehicleModelId).HasColumnName("VehicleModelID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FuelType).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.VehicleBrandId).HasColumnName("VehicleBrandID");

            entity.HasOne(d => d.VehicleBrand).WithMany(p => p.VehicleModels)
                .HasForeignKey(d => d.VehicleBrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VehicleModels_VehicleBrands");
        });

        modelBuilder.Entity<VehicleOptionalInformation>(entity =>
        {
            entity.HasKey(e => e.VehicleOptionalInformationId).HasName("VehicleOptionalInformation_pk");

            entity.ToTable("VehicleOptionalInformation");

            entity.Property(e => e.VehicleOptionalInformationId).HasColumnName("VehicleOptionalInformationID");
        });

        modelBuilder.Entity<VehicleStatistic>(entity =>
        {
            entity.HasKey(e => e.VehicleStatisticsId).HasName("VehicleStatistics_pk");

            entity.Property(e => e.VehicleStatisticsId).HasColumnName("VehicleStatisticsID");
            entity.Property(e => e.RentalRevenue).HasColumnType("money");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.VehicleTypeId).HasName("VehicleTypes_pk");

            entity.Property(e => e.VehicleTypeId).HasColumnName("VehicleTypeID");
            entity.Property(e => e.BaseDailyRate).HasColumnType("money");
            entity.Property(e => e.BaseDeposit).HasColumnType("money");
            entity.Property(e => e.BaseWeeklyRate).HasColumnType("money");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.RequiredLicenseType)
                .HasMaxLength(20)
                .HasDefaultValue("B");
        });

        //modelBuilder.Ignore<IdentityUser<int>>();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
