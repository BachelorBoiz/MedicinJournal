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
        Task<User?> GetByUserName(string userName);
        Task<User> CreateUserLogin(User user);
        Task<UserRole> GetUserRole(string userName);
        Task<User> GetUserByPatientId(int patientId);
        Task<User> GetUserByDoctorId(int doctorId);
        Task<SymmetricKey> GetSymmetricKeyByJournalId(int journalId);
        Task<SymmetricKey> CreateSymmetricKey(SymmetricKey key);
        Task<Signature> GetSignatureByJournalId(int journalId);
        Task<Signature> CreateSignature(Signature signature);
    }
}
