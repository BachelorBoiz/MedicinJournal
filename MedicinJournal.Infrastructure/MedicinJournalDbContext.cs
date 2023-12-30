using MedicinJournal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Infrastructure;

public class MedicinJournalDbContext : DbContext
{
    public DbSet<JournalEntity> Journals { get; set; }
    public DbSet<PatientEntity> Patients { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }

    public MedicinJournalDbContext(DbContextOptions<MedicinJournalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JournalEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<PatientEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<EmployeeEntity>().HasKey(x => x.Id);

        modelBuilder.Entity<PatientEntity>()
            .HasOne(user => user.Doctor)
            .WithMany(doctor => doctor.Patients)
            .HasForeignKey(user => user.DoctorId);

        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(employee => employee.Patients)
            .WithOne(patient => patient.Doctor)
            .HasForeignKey(patient => patient.DoctorId);

        modelBuilder.Entity<JournalEntity>()
            .HasOne(journal => journal.Patient)
            .WithMany(user => user.Journals)
            .HasForeignKey(journal => journal.PatientId);

        modelBuilder.Entity<JournalEntity>()
            .HasOne(journal => journal.Doctor)
            .WithMany()
            .HasForeignKey(journal => journal.DoctorId);
        }
}