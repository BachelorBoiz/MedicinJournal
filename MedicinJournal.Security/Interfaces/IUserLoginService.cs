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
        Task<User> CreateUserLogin(int userId, string userName, string plainTextPassword);
        Task<User?> GetUserLogin(string userName);
        Task<UserRole> GetUserRole(string userName);
        Task<User> GetUserByPatientId(int patientId);
        Task<User> GetUserByDoctorId(int doctorId);
        Task<SymmetricKey> GetSymmetricKeyByJournalId(int journalId);
        Task<SymmetricKey> CreateSymmetricKey(SymmetricKey key);
        Task<Signature> GetSignatureByJournalId(int journalId);
        Task<Signature> CreateSignature(Signature signature);
    }
}
