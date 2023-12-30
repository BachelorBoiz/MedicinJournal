using MedicinJournal.Core.Models;

namespace MedicinJournal.API.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public EmployeeRole Role { get; set; }
        public string Name { get; set; }
        public ICollection<PatientDto> Patients { get; set; }
    }
}
