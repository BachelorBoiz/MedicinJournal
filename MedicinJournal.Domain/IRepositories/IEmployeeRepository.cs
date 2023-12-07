using MedicinJournal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Domain.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee?> GetEmployee(string email);
    }
}
