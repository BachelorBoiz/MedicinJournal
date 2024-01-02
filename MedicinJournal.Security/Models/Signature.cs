using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Security.Models
{
    public class Signature
    {
        public int Id { get; set; }
        public int JournalId { get; set; }
        public byte[] EncryptedHash { get; set; }
        public DateTime TimeStamp { get; set; }
        //public UserRole Role { get; set; }
    }
}
