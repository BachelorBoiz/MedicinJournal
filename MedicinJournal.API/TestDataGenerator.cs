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

        //private readonly IUserLoginService _userLoginService;
        //private readonly IJournalService _journalService;
        //private readonly IUserService _userService;

        //public TestDataGenerator(IUserLoginService userLoginService, IUserService userService, IJournalService journalService)
        //{
        //    _userService = userService;
        //    _journalService = journalService;
        //    _userLoginService = userLoginService;
        //}

        public TestDataGenerator(MedicinJournalDbContext medicinJournalDbContext, SecurityDbContext userLoginDbContext, IPasswordHasher passwordHasher)
        {
            _medicinJournalDbContext = medicinJournalDbContext;
            _userLoginDbContext = userLoginDbContext;
            _passwordHasher = passwordHasher;
        }

        public void Generate()
        {
             _medicinJournalDbContext.Users.Add(new UserEntity
            {
                Name = "Dr Smith",
                Gender = "Male",
                BirthDate = DateTime.Now
            });

            _userLoginDbContext.UserLogins.Add(new UserLogin
            {
                UserId = 1,
                UserName = "Doctor",
                Role = UserRole.Doctor,
                HashedPassword = _passwordHasher.Hash("123456")
            });

            _medicinJournalDbContext.Users.Add(new UserEntity
            {
                Name = "Patient Zero",
                Gender = "Female",
                BirthDate = DateTime.Now
            });

            _userLoginDbContext.UserLogins.Add(new UserLogin
            {
                UserId = 2,
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
                EmployeeId = 1,
                PatientId = 2
            });

            _medicinJournalDbContext.SaveChanges();
            _userLoginDbContext.SaveChanges();
        }
    }
}
