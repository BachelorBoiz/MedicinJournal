using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Core.IServices
{
    public interface IPatientService
    {
        Task<Patient> GetByUserName(string userName);
        Task<Patient> CreatePatient(Patient patient);
    }
}
