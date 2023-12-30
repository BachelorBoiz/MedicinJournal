using MedicinJournal.Core.Models;

namespace MedicinJournal.API.Dtos
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public ICollection<JournalDto>? Journals { get; set; }
    }
}
