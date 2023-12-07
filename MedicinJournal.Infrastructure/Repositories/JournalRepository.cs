using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;
using MedicinJournal.Domain.IRepositories;

namespace MedicinJournal.Infrastructure.Repositories
{
    public class JournalRepository : IJournalRepository
    {
        public Task<Journal> GetJournalById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Journal> CreateJournal(Journal journal)
        {
            throw new NotImplementedException();
        }

        public Task<Journal> UpdateJournal(Journal journal)
        {
            throw new NotImplementedException();
        }

        public Task DeleteJournal(int id)
        {
            throw new NotImplementedException();
        }
    }
}
