using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Security.Models
{
    public class SymmetricKey
    {
        public int Id { get; set; }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public int JournalId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
