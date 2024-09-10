using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Data
{
    internal class HospitalDbContext : DbContext
    {
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
        public virtual DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }

        public HospitalDbContext()
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-J7QN3MV\\SQLEXPRESS;Initial Catalog=Hospital_Managementt;Integrated Security=True; TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .ToTable(nameof(Patient));
            modelBuilder.Entity<Patient>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Doctor>()
                .ToTable(nameof(Doctor));
            modelBuilder.Entity<Doctor>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Specialization>()
                .ToTable(nameof(Specialization));
            modelBuilder.Entity<Specialization>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Visit>()
                .ToTable(nameof(Visit));
            modelBuilder.Entity<Visit>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Visit>()
                .HasOne(e => e.Appointment)
                .WithOne(a => a.Visit)
                .HasForeignKey<Visit>(e => e.AppointmentId);
            modelBuilder.Entity<Visit>()
                .Property(e => e.TotalDue)
                .HasColumnType("money");

            modelBuilder.Entity<Appointment>()
                .ToTable(nameof(Appointment));
            modelBuilder.Entity<Appointment>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Appointment>()
                .HasOne(e => e.Visit)
                .WithOne(v => v.Appointment)
                .HasForeignKey<Visit>(e => e.AppointmentId);

            modelBuilder.Entity<DoctorSpecialization>()
                .ToTable(nameof(DoctorSpecialization));
            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
