using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public async Task<User?> GetByUserName(string userName)
        {
            return await _context.UserLogins.Where(e => e.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserLogin(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserRole> GetUserRole(string userName)
        {
            var user = await _context.UserLogins.FirstOrDefaultAsync(e => e.UserName == userName);

            return user.Role;
        }

        public async Task<User> GetUserByPatientId(int patientId)
        {
            var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.PatientId == patientId);

            return user;
        }

        public async Task<User> GetUserByDoctorId(int doctorId)
        {
            return await _context.UserLogins.FirstOrDefaultAsync(u => u.EmployeeId == doctorId);
        }

        public async Task<SymmetricKey> GetSymmetricKeyByJournalId(int journalId)
        {
            var symmetricKey = await _context.SymmetricKeys.FirstOrDefaultAsync(s => s.JournalId == journalId);

            return symmetricKey;
        }

        public async Task<SymmetricKey> CreateSymmetricKey(SymmetricKey key)
        {
            await _context.SymmetricKeys.AddAsync(key);
            await _context.SaveChangesAsync();

            return key;
        }

        public async Task<Signature> GetSignatureByJournalId(int journalId)
        {
            return await _context.Signatures.FirstOrDefaultAsync(s => s.JournalId == journalId);
        }

        public async Task<Signature> CreateSignature(Signature signature)
        {
            await _context.Signatures.AddAsync(signature);
            await _context.SaveChangesAsync();

            return signature;
        }
    }
}
