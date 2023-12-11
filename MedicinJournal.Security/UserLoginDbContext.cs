using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicinJournal.Security
{
    public class UserLoginDbContext : DbContext
    {
        public DbSet<UserLogin> UserLogins { get; set; }

        public UserLoginDbContext(DbContextOptions<UserLoginDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLogin>().HasKey(x => x.Id);
        }
    }
}
