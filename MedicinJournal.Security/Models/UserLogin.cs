using MedicinJournal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Security.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? EmployeeId { get; set; }
        public UserRole Role { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
    }
}
