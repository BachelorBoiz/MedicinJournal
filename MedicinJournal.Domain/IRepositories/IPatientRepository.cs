using MedicinJournal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Domain.IRepositories
{
    public interface IPatientRepository
    {
        Task<Patient> GetPatientById(int id);
        Task<Patient> CreatePatient(Patient patient);
    }
}
