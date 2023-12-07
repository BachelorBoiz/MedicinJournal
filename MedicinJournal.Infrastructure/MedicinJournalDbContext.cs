using MedicinJournal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Infrastructure;

public class MedicinJournalDbContext : DbContext
{
    public DbSet<PatientEntity> Patients { get; set; }
    public DbSet<EmployeeEntity> Employees{ get; set; }
    public DbSet<JournalEntity> Journals { get; set; }

    public MedicinJournalDbContext(DbContextOptions<MedicinJournalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<EmployeeEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<JournalEntity>().HasKey(x => x.Id);
    }
}