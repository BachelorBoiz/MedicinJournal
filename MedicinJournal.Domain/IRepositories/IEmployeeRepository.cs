using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Domain.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetById(int id);
    }
}
