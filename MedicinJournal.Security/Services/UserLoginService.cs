using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Models;

namespace MedicinJournal.Security.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IUserLoginRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserLoginService(IUserLoginRepository userLoginRepository, IPasswordHasher passwordHasher)
        {
            _repository = userLoginRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<UserLogin> CreateUserLogin(int userId, string userName, string plainTextPassword)
        {
            var hashedPassword = _passwordHasher.Hash(plainTextPassword);

            var userLogin = new UserLogin
            {
                UserId = userId,
                UserName = userName,
                HashedPassword = hashedPassword
            };

            return await _repository.CreateUserLogin(userLogin);
        }

        public async Task<UserLogin?> GetUserLogin(string userName)
        {
            return await _repository.GetByUserName(userName);
        }

        public async Task<UserRole> GetUserRole(string userName)
        {
            return await _repository.GetUserRole(userName);
        }
    }
}
