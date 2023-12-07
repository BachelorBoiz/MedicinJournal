using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Core.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public ICollection<Journal> Journals { get; set; }
    }
}
