using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Core.IServices
{
    public interface IJournalService
    {
        Task<Journal> GetJournalById(int id);
        Task<IEnumerable<Journal>> GetJournalsForUser(int userId);
        Task<Journal> CreateJournal (Journal journal);
        Task<Journal> UpdateJournal (Journal journal);
        Task DeleteJournal (int id);
    }
}
