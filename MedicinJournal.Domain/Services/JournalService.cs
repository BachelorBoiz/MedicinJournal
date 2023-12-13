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
    public class JournalService : IJournalService
    {
        private readonly IJournalRepository _journalRepository;

        public JournalService(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        public async Task<Journal> GetJournalById(int id)
        {
            return await _journalRepository.GetJournalById(id);
        }

        public async Task<IEnumerable<Journal>> GetJournalsForUser(int userId)
        {
            return await _journalRepository.GetJournalsForUser(userId);
        }

        public async Task<Journal> CreateJournal(Journal journal)
        {
            return await _journalRepository.CreateJournal(journal);
        }

        public async Task<Journal> UpdateJournal(Journal journal)
        {
            return await _journalRepository.UpdateJournal(journal);
        }

        public async Task DeleteJournal(int id)
        {
            await _journalRepository.DeleteJournal(id);
        }
    }
}
