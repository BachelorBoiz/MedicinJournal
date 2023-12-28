using AutoMapper;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MedicinJournalDbContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeRepository(MedicinJournalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Employee> GetById(int id)
        {
            var employeeEntity = await _dbContext.Employees
                .Include(e => e.Patients)
                .FirstOrDefaultAsync(e => e.Id == id);

            var employee = _mapper.Map<Employee>(employeeEntity);

            return employee;
        }
    }
}
