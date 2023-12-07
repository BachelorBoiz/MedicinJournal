using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
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
