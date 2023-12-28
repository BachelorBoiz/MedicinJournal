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
        public EmployeeRole Role { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }

    public enum EmployeeRole
    {
        Doctor,
        Receptionist,
        Nurse
    }
}
