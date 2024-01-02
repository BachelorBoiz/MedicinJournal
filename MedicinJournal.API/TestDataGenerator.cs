using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using MedicinJournal.Infrastructure.Entities;
using MedicinJournal.Security;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Models;
using PasswordManager.Infrastructure;

namespace MedicinJournal.API
{
    public class TestDataGenerator
    {
        private readonly MedicinJournalDbContext _medicinJournalDbContext;
        private readonly SecurityDbContext _userLoginDbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISymmetricCryptographyService _symmetricCryptographyService;
        private readonly IAsymmetricCryptographyService _asymmetricCryptographyService;

        public TestDataGenerator(
            MedicinJournalDbContext medicinJournalDbContext,
            SecurityDbContext userLoginDbContext,
            IPasswordHasher passwordHasher,
            ISymmetricCryptographyService symmetricCryptographyService,
            IAsymmetricCryptographyService asymmetricCryptographyService)
        {
            _medicinJournalDbContext = medicinJournalDbContext;
            _userLoginDbContext = userLoginDbContext;
            _passwordHasher = passwordHasher;
            _symmetricCryptographyService = symmetricCryptographyService;
            _asymmetricCryptographyService = asymmetricCryptographyService;
        }

        public void Generate()
        {
             _medicinJournalDbContext.Employees.Add(new EmployeeEntity
            {
                Name = "Dr Smith",
                Role = EmployeeRole.Doctor
            });

             _medicinJournalDbContext.SaveChanges();

             var doctorKeys = _asymmetricCryptographyService.GenerateKeyPair();

            _userLoginDbContext.UserLogins.Add(new User
            {
                EmployeeId = 1,
                UserName = "Doctor",
                Role = UserRole.Employee,
                HashedPassword = _passwordHasher.Hash("123456"),    
                PublicKey = doctorKeys.publicKey,
                PrivateKey = doctorKeys.privateKey
            });

            _medicinJournalDbContext.Patients.Add(new PatientEntity
            {
                Name = "Patient Zero",
                DoctorId = 1,
                Gender = "Female",
                BirthDate = DateTime.Now
            });

            var patientKeys = _asymmetricCryptographyService.GenerateKeyPair();

            _userLoginDbContext.UserLogins.Add(new User
            {
                PatientId = 1,
                UserName = "Patient",
                Role = UserRole.Patient,
                HashedPassword = _passwordHasher.Hash("123456"),
                PublicKey = patientKeys.publicKey,
                PrivateKey = patientKeys.privateKey
            });

            _medicinJournalDbContext.SaveChanges();

            var symmetricKey = _symmetricCryptographyService.GenerateKey();
            var iv = _symmetricCryptographyService.GenerateIV();

            _medicinJournalDbContext.Journals.Add(new JournalEntity
            {
                Created = DateTime.Now,
                Description = _symmetricCryptographyService.EncryptText(symmetricKey, iv, "This is the description text"),
                Title = "Test",
                PatientId = 1
            });

            _userLoginDbContext.SymmetricKeys.Add(new SymmetricKey
            {
                DoctorId = 1,
                PatientId = 1,
                JournalId = 1,
                IV = iv,
                Key = symmetricKey
            });

            _userLoginDbContext.Signatures.Add(new Signature
            {
                JournalId = 1,
                TimeStamp = DateTime.Now,
                EncryptedHash = _asymmetricCryptographyService.GenerateSignature("This is the description text",
                    _asymmetricCryptographyService.DeserializeRSAParameters(doctorKeys.privateKey)),
                
            });

            _medicinJournalDbContext.SaveChanges();
            _userLoginDbContext.SaveChanges();
        }
    }
}
