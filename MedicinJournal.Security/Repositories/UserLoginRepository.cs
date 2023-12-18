using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicinJournal.Security.Repositories
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly SecurityDbContext _context;

        public UserLoginRepository(SecurityDbContext context)
        {
            _context = context;
        }

        public async Task<UserLogin?> GetByUserName(string userName)
        {
            return await _context.UserLogins.Where(e => e.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<UserLogin> CreateUserLogin(UserLogin userLogin)
        {
            await _context.AddAsync(userLogin);
            await _context.SaveChangesAsync();

            return userLogin;
        }
    }
}
