namespace MedicinJournal.API.Dtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string PlainTextPassword { get; set; }
    }
}
