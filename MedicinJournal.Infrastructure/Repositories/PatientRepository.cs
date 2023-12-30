using AutoMapper;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;
using MedicinJournal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MedicinJournalDbContext _dbContext;
        private readonly IMapper _mapper;

        public PatientRepository(MedicinJournalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Patient?> GetPatientById(int id)
        {
            var patientEntity = await _dbContext.Patients
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .Include(p => p.Doctor)
                .Include(p => p.Journals)
                .FirstOrDefaultAsync(p => p.Id == id);

            var patient = _mapper.Map<Patient>(patientEntity);

            return patient;
        }

        public async Task<Patient> CreatePatient(Patient patient)
        {
            var entity = new PatientEntity
            {
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                Height = patient.Height,
                Weight = patient.Weight,
                Name = patient.Name
            };

            await _dbContext.Patients.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            patient.Id = entity.Id;

            return patient;
        }
    }
}
