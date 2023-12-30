namespace MedicinJournal.Infrastructure.Entities
{
    public class PatientEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public EmployeeEntity Doctor { get; set; }
        public int DoctorId { get; set; }
        public ICollection<JournalEntity>? Journals { get; set; }
    }
}
