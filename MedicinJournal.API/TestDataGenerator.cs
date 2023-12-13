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
        private readonly UserLoginDbContext _userLoginDbContext;
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

        public TestDataGenerator(MedicinJournalDbContext medicinJournalDbContext, UserLoginDbContext userLoginDbContext, IPasswordHasher passwordHasher)
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
                Role = UserRole.Doctor,
                BirthDate = DateTime.Now
            });

            _userLoginDbContext.UserLogins.Add(new UserLogin
            {
                UserId = 1,
                UserName = "Doctor",
                HashedPassword = _passwordHasher.Hash("123456")
            });

            _medicinJournalDbContext.Users.Add(new UserEntity
            {
                Name = "Patient Zero",
                Gender = "Female",
                Role = UserRole.Patient,
                BirthDate = DateTime.Now
            });

            _userLoginDbContext.UserLogins.Add(new UserLogin
            {
                UserId = 2,
                UserName = "Patient",
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

        //public async Task Generate()
        //{
        //    var doctor = await _userService.CreateUser(new User
        //    {
        //        Name = "Test",
        //        Gender = "Male",
        //        Role = UserRole.Doctor,
        //        BirthDate = DateTime.Now
        //    });

        //    await _userLoginService.CreateUserLogin(doctor.Id, "Doctor", "123456");

        //    var patient = await _userService.CreateUser(new User
        //    {
        //        Name = "Patient",
        //        Gender = "Female",
        //        Role = UserRole.Patient,
        //        BirthDate = DateTime.Now
        //    });

        //    await _userLoginService.CreateUserLogin(patient.Id, "Patient", "123456");

        //    await _journalService.CreateJournal(new Journal
        //    {
        //        Created = DateTime.Today,
        //        Description = "test",
        //        Employee = doctor,
        //        Patient = patient,
        //        Title = "Test journal"

        //    });
        //}
    }
}
