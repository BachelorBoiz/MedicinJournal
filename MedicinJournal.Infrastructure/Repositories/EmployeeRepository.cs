using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;
using PasswordManager.Infrastructure;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MedicinJournalDbContext _dbContext;

        public EmployeeRepository(MedicinJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Employee> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
