using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Domain.Services
{
    public class PatientService : IPatientService
    {
        public Task<Patient> GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<Patient> CreatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
