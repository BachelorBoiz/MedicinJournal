using MedicinJournal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Core.IServices
{
    public interface IEmployeeService
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee?> GetEmployee(string email);
    }
}
