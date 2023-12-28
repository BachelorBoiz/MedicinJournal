using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Core.Models;

namespace MedicinJournal.Infrastructure.Entities
{
    public class EmployeeEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EmployeeRole Role { get; set; }
        public ICollection<UserEntity> Patients { get; set; }
    }
}
