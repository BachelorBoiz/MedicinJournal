using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;

namespace MedicinJournal.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<User> CreateUser(User user)
        {
            return await _userRepository.CreateUser(user);
        }
    }
}
