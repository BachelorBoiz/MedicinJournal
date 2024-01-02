using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Core.Models
{
    public class Journal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public Patient Patient { get; set; }
    }
}
