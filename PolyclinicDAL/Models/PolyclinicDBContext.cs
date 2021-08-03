using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

#nullable disable

namespace PolyclinicDAL.Models
{
    public partial class PolyclinicDBContext : DbContext
    {
        public PolyclinicDBContext()
        {
        }

        public PolyclinicDBContext(DbContextOptions<PolyclinicDBContext> options)
            : base(options)
        {
        }

        [DbFunction("ufn_CalculateDoctorFees","dbo")]
        public static decimal GetDoctorFees(string doctorId, DateTime date)
        {
            return 0;
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<DoctorAppointment> DoctorAppointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var builder = new ConfigurationBuilder().
                                SetBasePath(Directory.GetCurrentDirectory()).
                                AddJsonFile("appsettings.json");
                var config = builder.Build();
                var connectionString = config.GetConnectionString("PolyclinicDBString");
                
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDbFunction(() => PolyclinicDBContext.GetDoctorFees());

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentNo)
                    .HasName("pk_AppointmentNo");

                entity.Property(e => e.DateofAppointment).HasColumnType("date");

                entity.Property(e => e.DoctorId)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DoctorID")
                    .IsFixedLength(true);

                entity.Property(e => e.PatientId)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("PatientID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DoctorID");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientID");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.DoctorId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DoctorID")
                    .IsFixedLength(true);

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fees).HasColumnType("money");

                entity.Property(e => e.Specialization)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("PatientID")
                    .IsFixedLength(true);

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
