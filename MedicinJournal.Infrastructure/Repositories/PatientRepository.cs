using MedicinJournal.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
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
