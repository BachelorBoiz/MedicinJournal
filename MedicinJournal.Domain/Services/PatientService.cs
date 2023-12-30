using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;

namespace MedicinJournal.Domain.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Patient> GetPatientById(int id)
        {
            return await _patientRepository.GetPatientById(id);
        }

        public async Task<Patient> CreatePatient(Patient patient)
        {
            return await _patientRepository.CreatePatient(patient);
        }
    }
}
