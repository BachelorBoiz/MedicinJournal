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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetById(int id)
        {
            return await _employeeRepository.GetById(id);
        }
    }
}
