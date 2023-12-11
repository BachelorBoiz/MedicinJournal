using MedicinJournal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
    }
}
