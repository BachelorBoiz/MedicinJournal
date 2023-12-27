using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Models;

namespace MedicinJournal.Security.Interfaces
{
    public interface IUserLoginRepository
    {
        Task<UserLogin?> GetByUserName(string userName);
        Task<UserLogin> CreateUserLogin(UserLogin userLogin);
        Task<UserRole> GetUserRole(int userId);
    }
}
