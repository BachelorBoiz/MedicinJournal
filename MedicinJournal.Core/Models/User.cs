using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserRole Role { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public ICollection<Journal>? Journals { get; set; }
    }

    public enum UserRole
    {
        Doctor = 1,
        Nurse = 2,
        Receptionist = 3,
        Patient = 4,
        Administrator = 5
    }
}
