using MedicinJournal.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicinJournal.Security
{
    public class SecurityDbContext : DbContext
    {
        public DbSet<User> UserLogins { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<SymmetricKey> SymmetricKeys { get; set; }

        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<AuditLog>().HasKey(x => x.Id);
            modelBuilder.Entity<Signature>().HasKey(x => x.Id);
            modelBuilder.Entity<SymmetricKey>().HasKey(x => x.Id);
        }
    }
}
