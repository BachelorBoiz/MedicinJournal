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
        public async Task<User> CreateUserLogin(int userId, string userName, string plainTextPassword)
        {
            var hashedPassword = _passwordHasher.Hash(plainTextPassword);

            var userLogin = new User
            {
                PatientId = userId,
                UserName = userName,
                HashedPassword = hashedPassword
            };

            return await _repository.CreateUserLogin(userLogin);
        }

        public async Task<User?> GetUserLogin(string userName)
        {
            return await _repository.GetByUserName(userName);
        }

        public async Task<UserRole> GetUserRole(string userName)
        {
            return await _repository.GetUserRole(userName);
        }

        public async Task<User> GetUserByPatientId(int patientId)
        {
            return await _repository.GetUserByPatientId(patientId);
        }

        public async Task<User> GetUserByDoctorId(int doctorId)
        {
            return await _repository.GetUserByDoctorId(doctorId);
        }

        public async Task<SymmetricKey> GetSymmetricKeyByJournalId(int journalId)
        {
            return await _repository.GetSymmetricKeyByJournalId(journalId);
        }

        public async Task<SymmetricKey> CreateSymmetricKey(SymmetricKey key)
        {
            return await _repository.CreateSymmetricKey(key);
        }

        public async Task<Signature> GetSignatureByJournalId(int journalId)
        {
            return await _repository.GetSignatureByJournalId(journalId);
        }

        public async Task<Signature> CreateSignature(Signature signature)
        {
            return await _repository.CreateSignature(signature);
        }
    }
}
