using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;
using MedicinJournal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class JournalRepository : IJournalRepository
    {
        private readonly MedicinJournalDbContext _dbContext;
        private readonly IMapper _mapper;

        public JournalRepository(MedicinJournalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Journal> GetJournalById(int id)
        {
            var journalEntity = _dbContext.Journals.FirstOrDefault(x => x.Id == id);

            var journal = _mapper.Map<Journal>(journalEntity);

            return journal;
        }

        public async Task<IEnumerable<Journal>> GetJournalsForUser(int userId)
        {
            var journalEntities = await _dbContext.Journals.Where(e => e.PatientId == userId).ToListAsync();

            var journals = _mapper.Map<IEnumerable<Journal>>(journalEntities);

            return journals;
        }

        public async Task<Journal> CreateJournal(Journal journal, int patientId)
        {
            var entity = _mapper.Map<JournalEntity>(journal);

            entity.PatientId = patientId;

            await _dbContext.Journals.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Journal>(entity);
        }

        public async Task<Journal> UpdateJournal(Journal journal)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteJournal(int id)
        {
            var journalEntity = await _dbContext.Journals.FindAsync(id);

            if(journalEntity == null)
            {
                throw new InvalidOperationException($"Journal entry with id: {id} not found");
            }

            _dbContext.Journals.Remove(journalEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
