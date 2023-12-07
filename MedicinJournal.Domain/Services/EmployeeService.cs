using MedicinJournal.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        public Task<Employee> AddEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task<Employee?> GetEmployee(string email)
        {
            throw new NotImplementedException();
        }
    }
}
