using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Core.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
