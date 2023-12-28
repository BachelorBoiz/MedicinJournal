using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Core.IServices
{
    public interface IEmployeeService
    {
        Task<Employee> GetById(int id);
    }
}
