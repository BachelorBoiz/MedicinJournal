using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Security.Models
{
    public enum UserRole
    {
        Doctor = 1,
        Nurse = 2,
        Receptionist = 3,
        Patient = 4,
        Administrator = 5
    }
}
