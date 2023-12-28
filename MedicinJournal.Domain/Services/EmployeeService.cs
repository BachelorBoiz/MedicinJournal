using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        public Task<Employee> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
