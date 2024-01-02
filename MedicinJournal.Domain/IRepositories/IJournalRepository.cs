using MedicinJournal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Domain.IRepositories
{
    public interface IJournalRepository
    {
        Task<Journal> GetJournalById(int id);
        Task<IEnumerable<Journal>> GetJournalsForUser(int userId);
        Task<Journal> CreateJournal(Journal journal, int patientId);
        Task<Journal> UpdateJournal(Journal journal);
        Task DeleteJournal(int id);
    }
}
