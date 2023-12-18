using MedicinJournal.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicinJournal.Security
{
    public class SecurityDbContext : DbContext
    {
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLogin>().HasKey(x => x.Id);
            modelBuilder.Entity<AuditLog>().HasKey(x => x.Id);
        }
    }
}
