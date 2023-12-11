using MedicinJournal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Infrastructure;

public class MedicinJournalDbContext : DbContext
{
    public DbSet<PatientEntity> Patients { get; set; }
    public DbSet<EmployeeEntity> Employees{ get; set; }
    public DbSet<JournalEntity> Journals { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    public MedicinJournalDbContext(DbContextOptions<MedicinJournalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<EmployeeEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<JournalEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<UserEntity>().HasKey(x => x.Id);

        modelBuilder.Entity<JournalEntity>()
            .HasOne(journal => journal.Patient) // Each JournalEntity has one Patient (UserEntity)
            .WithMany(user => user.Journals)    // Each UserEntity has many Journals
            .HasForeignKey(journal => journal.PatientId);        // Foreign key property in JournalEntity

        modelBuilder.Entity<JournalEntity>()
            .HasOne(journal => journal.Employee)
            .WithMany() // No need to specify the navigation property on the "many" side
            .HasForeignKey(journal => journal.EmployeeId);       // Foreign key property in JournalEntity

    }
}