using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Models;

namespace MedicinJournal.Security.Interfaces
{
    public interface IUserLoginService
    {
        Task<UserLogin> CreateUserLogin(int userId, string userName, string plainTextPassword);
        Task<UserLogin?> GetUserLogin(string userName);
        Task<UserRole> GetUserRole(string userName);
    }
}
