using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Security.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public required string EntityName { get; set; }
        public required string Action { get; set; }
        
        public DateTime Timestamp { get; set; }
        public required string Changes { get; set; }
    }
}
