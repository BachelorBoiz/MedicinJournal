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

        public TestDataGenerator(MedicinJournalDbContext medicinJournalDbContext, SecurityDbContext userLoginDbContext, IPasswordHasher passwordHasher)
        {
            _medicinJournalDbContext = medicinJournalDbContext;
            _userLoginDbContext = userLoginDbContext;
            _passwordHasher = passwordHasher;
        }

        public void Generate()
        {
             _medicinJournalDbContext.Employees.Add(new EmployeeEntity
            {
                Name = "Dr Smith",
                Role = EmployeeRole.Doctor
            });

             _medicinJournalDbContext.SaveChanges();

            _userLoginDbContext.UserLogins.Add(new UserLogin
            {
                EmployeeId = 1,
                UserName = "Doctor",
                Role = UserRole.Employee,
                HashedPassword = _passwordHasher.Hash("123456")
            });

            _medicinJournalDbContext.Patients.Add(new PatientEntity
            {
                Name = "Patient Zero",
                DoctorId = 1,
                Gender = "Female",
                BirthDate = DateTime.Now
            });

            _userLoginDbContext.UserLogins.Add(new UserLogin
            {
                PatientId = 1,
                UserName = "Patient",
                Role = UserRole.Patient,
                HashedPassword = _passwordHasher.Hash("123456")
            });

            _medicinJournalDbContext.SaveChanges();

            _medicinJournalDbContext.Journals.Add(new JournalEntity
            {
                Created = DateTime.Now,
                Description = "Test",
                Title = "Test",
                DoctorId  = 1,
                PatientId = 1
            });

            _medicinJournalDbContext.SaveChanges();
            _userLoginDbContext.SaveChanges();
        }
    }
}
