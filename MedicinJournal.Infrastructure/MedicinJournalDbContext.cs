using MedicinJournal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Infrastructure;

public class MedicinJournalDbContext : DbContext
{
    public DbSet<JournalEntity> Journals { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }

    public MedicinJournalDbContext(DbContextOptions<MedicinJournalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JournalEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<UserEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<EmployeeEntity>().HasKey(x => x.Id);

        modelBuilder.Entity<UserEntity>()
            .HasOne(user => user.Doctor)
            .WithMany(doctor => doctor.Patients)
            .HasForeignKey(user => user.DoctorId);

        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(employee => employee.Patients)
            .WithOne(patient => patient.Doctor)
            .HasForeignKey(patient => patient.DoctorId);

        modelBuilder.Entity<JournalEntity>()
            .HasOne(journal => journal.Patient) // Each JournalEntity has one Patient (UserEntity)
            .WithMany(user => user.Journals)    // Each UserEntity has many Journals
            .HasForeignKey(journal => journal.PatientId);        // Foreign key property in JournalEntity

        modelBuilder.Entity<JournalEntity>()
            .HasOne(journal => journal.Doctor)
            .WithMany() // No need to specify the navigation property on the "many" side
            .HasForeignKey(journal => journal.DoctorId);       // Foreign key property in JournalEntity
        }
}